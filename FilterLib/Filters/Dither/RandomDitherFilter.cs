using FilterLib.Reporting;
using FilterLib.Util;
using Random = System.Random;

namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Random dither filter.
    /// </summary>
    [Filter]
    public sealed class RandomDitherFilter : FilterInPlaceBase
    {
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
            Random rnd = new(Seed);
            float intervalSize = 255f / (levels - 1);
            fixed (byte* start = image)
            {
                byte* ptr = start;
                // Iterate through each pixel and process individually
                for (int y = 0; y < image.Height; ++y)
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
                    reporter?.Report(y + 1, 0, image.Height);
                }
            }
            reporter?.Done();
        }
    }
}
