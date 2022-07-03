using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;
using MathF = System.MathF;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Downscale an image by averaging all the pixels in the original image that
    /// correspond to a pixel in the new image.
    /// </summary>
    [Filter]
    public sealed class BoxDownscaleFilter : FilterBase
    {
        /// <summary>
        /// New width, must be less than or equal to original.
        /// </summary>
        [FilterParam]
        public Size Width { get; set; }

        /// <summary>
        /// New height, must be less than or equal to original.
        /// </summary>
        [FilterParam]
        public Size Height { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">New width, must be less than or equal to original</param>
        /// <param name="height">New height, must be less than or equal to original</param>
        public BoxDownscaleFilter(Size width, Size height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BoxDownscaleFilter() : this(Size.Relative(1f), Size.Relative(1f)) { }

        /// <inheritdoc/>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            int newWidth = Width.ToAbsolute(image.Width);
            int newHeight = Height.ToAbsolute(image.Height);

            if (newWidth <= 0) throw new System.ArgumentException($"Non-positive new width: {newHeight}.");
            if (newHeight <= 0) throw new System.ArgumentException($"Non-positive new height: {newHeight}.");
            if (newWidth > image.Width) throw new System.ArgumentException($"New width {newWidth} is greater than old width {image.Width}");
            if (newHeight > image.Height) throw new System.ArgumentException($"New height {newHeight} is greater than old height {image.Height}");

            Bitmap resized = new(newWidth, newHeight);
            float xRatio = image.Width / (float)resized.Width;
            float yRatio = image.Height / (float)resized.Height;
            System.Diagnostics.Debug.Assert(xRatio >= 0.9999f);
            System.Diagnostics.Debug.Assert(yRatio >= 0.9999f);
            using DisposableBitmapData bmd = new(resized, PixelFormat.Format24bppRgb);
            using DisposableBitmapData bmdOrig = new(image, PixelFormat.Format24bppRgb);
            unsafe
            {
                // Loop through pixels of the resized image
                for (int y = 0; y < resized.Height; ++y)
                {
                    byte* row = (byte*)bmd.Scan0 + y * bmd.Stride;
                    for (int x = 0; x < resized.Width; ++x)
                    {
                        // Each pixel corresponds to a "box" in the original image, covering
                        // multiple pixels so we do an average. Some pixels might be partially covered
                        // if the ratio is not an integer. Hence, we round down the starting pixel and
                        // round up the ending pixel, and check for partial cover inside the loop.
                        float rSum = 0, gSum = 0, bSum = 0, weightSum = 0;

                        float yStart = y * yRatio;
                        float yEnd = (y + 1) * yRatio;
                        float yEndCeil = MathF.Ceiling(yEnd);
                        for (int yi = (int)Math.Floor(yStart); yi < yEndCeil; ++yi)
                        {
                            float wy = 1;
                            // Check for partially covered pixel in the beginning and end
                            if (yi < yStart) wy = 1 - (yStart - yi);
                            else if (yi + 1 > yEnd) wy = yEnd - yi;

                            float xStart = x * xRatio;
                            float xEnd = (x + 1) * xRatio;
                            float xEndCeil = MathF.Ceiling(xEnd);
                            for (int xi = (int)Math.Floor(xStart); xi < xEndCeil; ++xi)
                            {
                                // Check for partially covered pixel in the beginning and end
                                float wx = 1;
                                if (xi < xStart) wx = 1 - (xStart - xi);
                                else if (xi + 1 > xEnd) wx = xEnd - xi;

                                byte* pxOrig = (byte*)bmdOrig.Scan0 + yi * bmdOrig.Stride + (xi * 3);
                                rSum += pxOrig[2] * wx * wy;
                                gSum += pxOrig[1] * wx * wy;
                                bSum += pxOrig[0] * wx * wy;
                                weightSum += wx * wy;
                            }
                        }

                        int x_3 = x * 3;
                        row[x_3] = (bSum / weightSum).ClampToByte();
                        row[x_3 + 1] = (gSum / weightSum).ClampToByte();
                        row[x_3 + 2] = (rSum / weightSum).ClampToByte();

                    }
                    reporter?.Report(y, 0, resized.Height - 1);
                }
            }

            reporter?.Done();
            return resized;
        }
    }
}
