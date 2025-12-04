using FilterLib.Reporting;
using FilterLib.Util;
using Math = System.Math;
using Parallel = System.Threading.Tasks.Parallel;
using Random = System.Random;

namespace FilterLib.Filters.Artistic
{
    [Filter("Replace each pixel with a random one within a given radius.")]
    public sealed class RandomJitterFilter : FilterInPlaceBase
    {
        private const int MAX_THREADS = 128;

        /// <summary>
        /// Jitter radius [0;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int Radius
        {
            get;
            set { field = System.Math.Max(0, value); }
        }

        /// <summary>
        /// Random number generator seed.
        /// </summary>
        [FilterParam]
        public int Seed { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="radius">Jitter radius [0;...]</param>
        /// <param name="seed">Random number generator seed</param>
        public RandomJitterFilter(int radius = 0, int seed = 0)
        {
            Radius = radius;
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
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(image.Width == original.Width);
            int width_3 = image.Width * 3;

            fixed (byte* newStart = image, oldStart = original)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                Parallel.For(0, threads, i =>
                {
                    int yStart = threadSize * i;
                    int yEnd = (i == threads - 1) ? image.Height : yStart + threadSize;
                    Random rnd = rndp[i];
                    for (int y = yStart; y < yEnd; ++y)
                    {
                        byte* newRow = newStart0 + y * width_3;
                        byte* oldRow = oldStart0 + y * width_3;
                        for (int x = 0; x < width_3; x += 3)
                        {
                            // Random numbers between -radius and +radius
                            int dx = rnd.Next(2 * Radius + 1) - Radius;
                            int dy = rnd.Next(2 * Radius + 1) - Radius;

                            // When out of range, take zero instead
                            if (x / 3 + dx < 0 || x / 3 + dx >= image.Width) dx = 0;
                            if (y + dy < 0 || y + dy >= image.Height) dy = 0;

                            // Calculate offset (dy rows, dx columns)
                            int idx = dy * width_3 + dx * 3;

                            newRow[x] = oldRow[x + idx];
                            newRow[x + 1] = oldRow[x + idx + 1];
                            newRow[x + 2] = oldRow[x + idx + 2];
                        }
                        reporter?.Report(y + 1, 0, image.Height);
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, threads);
                });
            }
            reporter?.Done();
        }
    }
}

