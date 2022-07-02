using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Mosaic
{
    /// <summary>
    /// Pixelate filter.
    /// </summary>
    [Filter]
    public sealed class PixelateFilter : FilterInPlaceBase
    {
        public enum PixelateMode { Average, MidPoint }

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
        /// Pixelating mode.
        /// </summary>
        [FilterParam]
        public PixelateMode Mode { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="size">Block size[1;...]</param>
        /// <param name="mode">Pixelating mode</param>
        public PixelateFilter(int size = 1, PixelateMode mode = PixelateMode.Average)
        {
            Size = size;
            Mode = mode;
        }

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone original image for iteration
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3;
                int h = image.Height;
                int x, y, xSub, ySub, sizeMul3 = size * 3, rSum, gSum, bSum, n;
                byte rNew, gNew, bNew;
                
                unsafe
                {
                    // Iterate through block rows
                    for (y = 0; y < h; y += size)
                    {
                        // Iterate through block columns
                        for (x = 0; x < wMul3; x += sizeMul3)
                        {
                            byte* row;
                            // Determine block color based on mode
                            switch (Mode)
                            {
                                case PixelateMode.Average:
                                    rSum = gSum = bSum = n = 0;
                                    for (ySub = 0; ySub < size && y + ySub < h; ++ySub)
                                    {
                                        // Get row
                                        row = (byte*)bmd.Scan0 + ((y + ySub) * bmd.Stride);
                                        for (xSub = 0; xSub < sizeMul3 && x + xSub < wMul3; xSub += 3)
                                        {
                                            rSum += row[x + xSub + 2];
                                            gSum += row[x + xSub + 1];
                                            bSum += row[x + xSub];
                                            ++n;
                                        }
                                    }
                                    rNew = (byte)(rSum / n);
                                    gNew = (byte)(gSum / n);
                                    bNew = (byte)(bSum / n);
                                    break;
                                case PixelateMode.MidPoint:
                                    row = (byte*)bmd.Scan0 + (Math.Min(y + size / 2, h - 1) * bmd.Stride);
                                    int xMid = Math.Min(x + (size / 2) * 3, wMul3 - 3);
                                    rNew = row[xMid + 2];
                                    gNew = row[xMid + 1];
                                    bNew = row[xMid];
                                    break;
                                default:
                                    throw new System.ArgumentException($"Unknown pixelate mode: {Mode}");
                            }

                            // Fill block
                            for (ySub = 0; ySub < size && y + ySub < h; ++ySub)
                            {
                                // Get row
                                row = (byte*)bmd.Scan0 + ((y + ySub) * bmd.Stride);
                                for (xSub = 0; xSub < sizeMul3 && x + xSub < wMul3; xSub += 3)
                                {
                                    row[x + xSub + 2] = rNew;
                                    row[x + xSub + 1] = gNew;
                                    row[x + xSub] = bNew;
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
