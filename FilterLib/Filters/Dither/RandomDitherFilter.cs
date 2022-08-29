using FilterLib.Reporting;
using FilterLib.Util;
using Random = System.Random;
using Math = System.Math;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Random dither filter.
    /// </summary>
    [Filter]
    public sealed class RandomDitherFilter : FilterInPlaceBase
    {
        private const int MAX_THREADS = 128;

        private int levels;

        /// <summary>
        /// Number of levels [2:256].
        /// </summary>
        [FilterParam]
        [FilterParamMin(2)]
        [FilterParamMax(256)]
        public int Levels
        {
            get { return levels; }
            set { levels = value.Clamp(2, 256); }
        }


        /// <summary>
        /// Random number generator seed
        /// </summary>
        [FilterParam]
        public int Seed { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="levels">Levels [2;256]</param>
        /// <param name="seed">Random generator seed</param>
        public RandomDitherFilter(int levels = 256, int seed = 0)
        {
            Levels = levels;
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
            float intervalSize = 255f / (levels - 1);
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
                            float nextRnd = rnd.NextSingle();

                            for (int c = 0; c < 3; ++c)
                            {
                                float floor = System.MathF.Floor(*ptr / intervalSize) * intervalSize;
                                float ceil = floor + intervalSize;
                                *ptr = ((floor + nextRnd * intervalSize > *ptr) ? floor : ceil).ClampToByte();
                                ++ptr;
                            }
                        }
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, threads);
                });
            }
            reporter?.Done();
        }
    }
}
