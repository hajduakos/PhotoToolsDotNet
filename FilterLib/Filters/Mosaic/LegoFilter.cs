using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace FilterLib.Filters.Mosaic
{
    /// <summary>
    /// Lego filter.
    /// </summary>
    [Filter]
    public sealed class LegoFilter : FilterInPlaceBase
    {
        private int size;

        /// <summary>
        /// Block size property [8;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(8)]
        public int Size
        {
            get { return size; }
            set { size = Math.Max(8, value); }
        }

        /// <summary>
        /// Constructor with block size.
        /// </summary>
        /// <param name="size">Block size</param>
        public LegoFilter(int size = 16) => Size = size;

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Create shadow pattern for a brick
            using (Bitmap pattern = new(size, size))
            {
                using (Graphics gfx = Graphics.FromImage(pattern))
                {
                    gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    // Top glow circle
                    using (LinearGradientBrush lgb = new(
                        new Point(size / 4 - 1, size / 4 - 1), new Point(size / 4 - 1, size / 2 + 1),
                        System.Drawing.Color.FromArgb(255, 255, 255, 255), System.Drawing.Color.FromArgb(0, 255, 255, 255)))
                    {
                        gfx.Clip = new Region(new Rectangle(0, 0, size, size / 2));
                        gfx.FillEllipse(lgb, size / 4 - 1, size / 4 - 1, size / 2 + 2, size / 2 + 2);
                    }
                    // Bottom shadow circle
                    using (LinearGradientBrush lgb = new(
                            new Point(0, size / 2), new Point(0, 3 * size / 4 + 1),
                            System.Drawing.Color.FromArgb(0, 0, 0, 0), System.Drawing.Color.FromArgb(255, 0, 0, 0)))
                    {
                        gfx.Clip = new Region(new Rectangle(0, size / 2 + 1, size, size / 2));
                        gfx.FillEllipse(lgb, size / 4 - 1, size / 4 - 1, size / 2 + 2, size / 2 + 2);
                    }

                }
                // Clone original image for iteration
                using Bitmap original = (Bitmap)image.Clone();
                using DisposableBitmapData bmd = new(original, PixelFormat.Format24bppRgb);
                int width_3 = original.Width * 3;
                int h = original.Height;
                int x, y, xSub, ySub, sizeMul3 = size * 3, rSum, gSum, bSum, n;
                // Create graphics from image to draw on
                using (Graphics gfx = Graphics.FromImage(image))
                {
                    unsafe
                    {
                        // Iterate through block rows
                        for (y = 0; y < h; y += size)
                        {
                            // Iterate through block columns
                            for (x = 0; x < width_3; x += sizeMul3)
                            {
                                rSum = gSum = bSum = n = 0; // Clear sums
                                for (ySub = 0; ySub < size && y + ySub < h; ++ySub)
                                {
                                    // Get row
                                    byte* row = (byte*)bmd.Scan0 + ((y + ySub) * bmd.Stride);
                                    for (xSub = 0; xSub < sizeMul3 && x + xSub < width_3; xSub += 3)
                                    {
                                        rSum += row[x + xSub + 2];
                                        gSum += row[x + xSub + 1];
                                        bSum += row[x + xSub];
                                        ++n;
                                    }
                                }
                                // Get pixel color
                                using Brush pixel = new SolidBrush(System.Drawing.Color.FromArgb(rSum / n, gSum / n, bSum / n));
                                gfx.SmoothingMode = SmoothingMode.None;
                                gfx.FillRectangle(pixel, x / 3, y, size, size); // Pixelerate
                                gfx.DrawImage(pattern, x / 3, y, size, size); // Draw brick pattern
                                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                                gfx.FillEllipse(pixel, x / 3 + size / 4, y + size / 4, size / 2, size / 2); // Fill center circle
                            }
                            if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                        }
                    }
                }

            }

            reporter?.Done();
        }
    }
}
