using FilterLib.Reporting;
using Math = System.Math;

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
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            unsafe
            {
                fixed (byte* start = image)
                {
                    int width_3 = image.Width * 3;
                    int h = image.Height;
                    int x, y, xSub, ySub, size_3 = size * 3, rSum, gSum, bSum, n;
                    byte rNew, gNew, bNew;

                    // Iterate through block rows
                    for (y = 0; y < h; y += size)
                    {
                        // Iterate through block columns
                        for (x = 0; x < width_3; x += size_3)
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
                                        row = start + ((y + ySub) * width_3);
                                        for (xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
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
                                    row = start + (Math.Min(y + size / 2, h - 1) * width_3);
                                    int xMid = Math.Min(x + (size / 2) * 3, width_3 - 3);
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
                                row = start + ((y + ySub) * width_3);
                                for (xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
                                {
                                    row[x + xSub + 2] = rNew;
                                    row[x + xSub + 1] = gNew;
                                    row[x + xSub] = bNew;
                                }
                            }
                        }
                        reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
