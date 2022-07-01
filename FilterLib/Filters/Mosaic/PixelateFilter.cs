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
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3; // Width of a row
                int h = image.Height; // Image height
                int x, y, xSub, ySub, sizeMul3 = size * 3, rSum, gSum, bSum, n;
                byte rAvg, gAvg, bAvg;
                
                unsafe
                {
                    // Iterate through block rows
                    for (y = 0; y < h; y += size)
                    {
                        // Iterate through block columns
                        for (x = 0; x < wMul3; x += sizeMul3)
                        {
                            // Calculate average color
                            rSum = gSum = bSum = n = 0;
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
                            rAvg = (byte)(rSum / n);
                            gAvg = (byte)(gSum / n);
                            bAvg = (byte)(bSum / n);

                            // Fill with average
                            for (ySub = 0; ySub < size && y + ySub < h; ++ySub)
                            {
                                // Get row
                                byte* row = (byte*)bmd.Scan0 + ((y + ySub) * bmd.Stride);
                                for (xSub = 0; xSub < sizeMul3 && x + xSub < wMul3; xSub += 3)
                                {
                                    row[x + xSub + 2] = rAvg;
                                    row[x + xSub + 1] = gAvg;
                                    row[x + xSub] = bAvg;
                                }
                            }
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
