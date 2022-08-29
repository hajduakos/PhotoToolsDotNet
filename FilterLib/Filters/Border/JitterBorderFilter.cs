using FilterLib.Reporting;
using FilterLib.Util;
using System;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Add a jitter border, where the probability of coloring
    /// a pixel gradually goes from 1 to 0 towards the center.
    /// </summary>
    [Filter]
    public sealed class JitterBorderFilter : FilterInPlaceBase
    {
        private const int MAX_THREADS = 128;

        /// <summary>
        /// Border width.
        /// </summary>
        [FilterParam]
        public Util.Size Width { get; set; }

        /// <summary>
        /// Border color.
        /// </summary>
        [FilterParam]
        public RGB Color { get; set; }

        /// <summary>
        /// Random generator seed.
        /// </summary>
        [FilterParam]
        public int Seed { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public JitterBorderFilter() : this(Util.Size.Absolute(0), new RGB(0, 0, 0), 0) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Border width</param>
        /// <param name="color">Border color</param>
        /// <param name="seed">Random generator seed</param>
        public JitterBorderFilter(Util.Size width, RGB color, int seed = 0)
        {
            Width = width;
            Color = color;
            Seed = seed;
        }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            int threads = Math.Min(image.Height, MAX_THREADS);
            int threadSize = image.Height / threads;
            RandomPool rndp = new(threads, Seed);

            int borderWidth = Width.ToAbsolute(Math.Max(image.Width, image.Height));
            // Starting from the edge of the image, and going inwards, the probability of
            // coloring a pixel fades off in with a sine transition for smooth results
            float[] map = new float[borderWidth + 1];
            Parallel.For(0, borderWidth + 1, x =>
                  map[x] = MathF.Sin(x / (float)borderWidth * MathF.PI - MathF.PI / 2) / 2 + .5f);

            fixed (byte* start = image)
            {
                byte* start0 = start;
                Parallel.For(0, threads, i =>
                {
                    int yStart = threadSize * i;
                    int yEnd = (i == threads - 1) ? image.Height : yStart + threadSize;
                    byte* ptr = start0 + yStart * image.Width * 3;
                    Random rnd = rndp[i];
                    for (int y = yStart; y < yEnd; ++y)
                    {
                        for (int x = 0; x < image.Width; ++x)
                        {
                            int distanceFromNearestEdge = Math.Min(Math.Min(x, y), Math.Min(image.Width - x - 1, image.Height - y - 1));
                            if (distanceFromNearestEdge <= borderWidth && rnd.NextSingle() > map[distanceFromNearestEdge])
                            {
                                ptr[0] = Color.R;
                                ptr[1] = Color.G;
                                ptr[2] = Color.B;
                            }
                            ptr += 3;
                        }
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, threads);
                });
            }
            reporter?.Done();
        }
    }
}