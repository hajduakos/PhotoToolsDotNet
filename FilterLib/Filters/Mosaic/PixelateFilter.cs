using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterLib.Filters.Mosaic
{
    /// <summary>
    /// Pixelate filter.
    /// </summary>
    [Filter]
    public sealed class PixelateFilter : FilterInPlaceBase
    {
        private int size;

        /// <summary>
        /// Pixel size [1;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(1)]
        public int Size
        {
            get { return size; }
            set { size = Math.Max(1, value); }
        }

        /// <summary>
        /// Constructor with block size.
        /// </summary>
        /// <param name="size">Block size[1;...]</param>
        public PixelateFilter(int size = 1) => Size = size;

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone original image for iteration
            using (Bitmap original = (Bitmap)image.Clone())
            using (DisposableBitmapData bmd = new(original, PixelFormat.Format24bppRgb))
            using (Graphics gfx = Graphics.FromImage(image))
            {
                int wMul3 = original.Width * 3; // Width of a row
                int h = original.Height; // Image height
                int x, y, xSub, ySub, sizeMul3 = size * 3, rSum, gSum, bSum, n;
                
                unsafe
                {
                    // Iterate through block rows
                    for (y = 0; y < h; y += size)
                    {
                        // Iterate through block columns
                        for (x = 0; x < wMul3; x += sizeMul3)
                        {
                            rSum = gSum = bSum = n = 0; // Clear sums
                            for (ySub = 0; ySub < size && y + ySub < h; ++ySub)
                            {
                                // Get row
                                byte* row = (byte*)bmd.Scan0 + ((y + ySub) * bmd.Stride);
                                for (xSub = 0; xSub < sizeMul3 && x + xSub < wMul3; xSub += 3)
                                {
                                    rSum += row[x + xSub + 2];
                                    gSum += row[x + xSub + 1];
                                    bSum += row[x + xSub];
                                    ++n;
                                }
                            }
                            // Get pixel color
                            using Brush pixel = new SolidBrush(System.Drawing.Color.FromArgb(rSum / n, gSum / n, bSum / n));
                            gfx.FillRectangle(pixel, x / 3, y, size, size); // Pixelate
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
