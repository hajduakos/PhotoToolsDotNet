using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Resize filter.
    /// </summary>
    [Filter]
    public sealed class ResizeFilter : FilterBase
    {
        /// <summary>
        /// New width.
        /// </summary>
        [FilterParam]
        public Util.Size Width { get; set; }

        /// <summary>
        /// New height.
        /// </summary>
        [FilterParam]
        public Util.Size Height { get; set; }

        /// <summary>
        /// Interpolation mode.
        /// </summary>
        [FilterParam]
        public InterpolationMode Interpolation { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        /// <param name="interpolation">Interpolation mode</param>
        public ResizeFilter(Util.Size width, Util.Size height, InterpolationMode interpolation = InterpolationMode.NearestNeighbor)
        {
            Width = width;
            Height = height;
            Interpolation = interpolation;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ResizeFilter() : this(Util.Size.Relative(1f), Util.Size.Relative(1f)) { }

        /// <inheritdoc/>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            int newWidth = Width.ToAbsolute(image.Width);
            int newHeight = Height.ToAbsolute(image.Height);

            if (newWidth <= 0) throw new System.ArgumentException($"Invalid new width: {newWidth}.");
            if (newHeight <= 0) throw new System.ArgumentException($"Invalid new height: {newHeight}.");

            Bitmap resized = new(newWidth, newHeight);
            using (DisposableBitmapData bmd = new(resized, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdOrig = new(image, PixelFormat.Format24bppRgb))
            {
                int width_3 = resized.Width * 3;
                int h = resized.Height;
                int x, y;
                int x0, y0, x1, y1;
                // We want to map [0; w-1] to [0; w'-1], hence the (-1) adjustment
                float wScale = resized.Width > 1 ? (image.Width - 1) / (float)(resized.Width - 1) : 0;
                float hScale = resized.Height > 1 ? (image.Height - 1) / (float)(resized.Height - 1) : 0;

                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            switch (Interpolation)
                            {
                                case InterpolationMode.NearestNeighbor:
                                    y0 = (int)Math.Round(y * hScale);
                                    x0 = (int)Math.Round(x / 3 * wScale) * 3;
                                    byte* rowOrig = (byte*)bmdOrig.Scan0 + (y0 * bmdOrig.Stride);
                                    for (int i = 0; i < 3; i++) row[x + i] = rowOrig[x0 + i];
                                    break;
                                case InterpolationMode.Bilinear:
                                    float yf = y * hScale;
                                    y0 = (int)Math.Floor(yf);
                                    y1 = (int)Math.Ceiling(yf);
                                    float yRatio1 = yf - y0;
                                    float yRatio0 = 1 - yRatio1;
                                    float xf = x / 3 * wScale;
                                    x0 = (int)Math.Floor(xf) * 3;
                                    x1 = (int)Math.Ceiling(xf) * 3;
                                    float xRatio1 = xf - x0 / 3;
                                    float xRatio0 = 1 - xRatio1;
                                    byte* rowOrig0 = (byte*)bmdOrig.Scan0 + (y0 * bmdOrig.Stride);
                                    byte* rowOrig1 = (byte*)bmdOrig.Scan0 + (y1 * bmdOrig.Stride);
                                    for (int i = 0; i < 3; i++)
                                        row[x + i] = (byte)(
                                            yRatio0 * xRatio0 * rowOrig0[x0 + i] +
                                            yRatio1 * xRatio0 * rowOrig1[x0 + i] +
                                            yRatio0 * xRatio1 * rowOrig0[x1 + i] +
                                            yRatio1 * xRatio1 * rowOrig1[x1 + i]);
                                    break;
                                default:
                                    throw new System.ArgumentException($"Unknown interpolation mode: {Interpolation}");
                            }
                        }
                        reporter?.Report(y, 0, h - 1);
                    }
                }
            }

            reporter?.Done();
            return resized;
        }
    }
}
