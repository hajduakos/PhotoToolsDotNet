using FilterLib.Reporting;
using Math = System.Math;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Mosaic
{
    /// <summary>
    /// Pixelate the image to bigger blocks.
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
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            int width_3 = image.Width * 3;
            int size_3 = size * 3;
            fixed (byte* start = image)
            {
                byte* start0 = start;
                int yMax = image.Height / size;
                if (yMax * size < image.Height) yMax++;
                // Iterate through block rows
                Parallel.For(0, yMax, y =>
                {
                    y *= size;
                    byte rNew, gNew, bNew;
                    // Iterate through block columns
                    for (int x = 0; x < width_3; x += size_3)
                    {
                        byte* row;
                        // Determine block color based on mode
                        switch (Mode)
                        {
                            case PixelateMode.Average:
                                float rSum = 0, gSum = 0, bSum = 0;
                                int n = 0;
                                for (int ySub = 0; ySub < size && y + ySub < image.Height; ++ySub)
                                {
                                    row = start0 + ((y + ySub) * width_3);
                                    for (int xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
                                    {
                                        rSum += row[x + xSub];
                                        gSum += row[x + xSub + 1];
                                        bSum += row[x + xSub + 2];
                                        ++n;
                                    }
                                }
                                rNew = (byte)(rSum / n);
                                gNew = (byte)(gSum / n);
                                bNew = (byte)(bSum / n);
                                break;
                            case PixelateMode.MidPoint:
                                row = start0 + (Math.Min(y + size / 2, image.Height - 1) * width_3);
                                int xMid = Math.Min(x + (size / 2) * 3, width_3 - 3);
                                rNew = row[xMid];
                                gNew = row[xMid + 1];
                                bNew = row[xMid + 2];
                                break;
                            default:
                                throw new System.ArgumentException($"Unknown pixelate mode: {Mode}");
                        }

                        // Fill block
                        for (int ySub = 0; ySub < size && y + ySub < image.Height; ++ySub)
                        {
                            row = start0 + ((y + ySub) * width_3);
                            for (int xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
                            {
                                row[x + xSub] = rNew;
                                row[x + xSub + 1] = gNew;
                                row[x + xSub + 2] = bNew;
                            }
                        }
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, yMax);
                });
            }
            reporter?.Done();
        }
    }
}
