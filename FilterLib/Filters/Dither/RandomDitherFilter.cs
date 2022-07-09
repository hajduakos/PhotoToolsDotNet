using FilterLib.Util;
using Random = System.Random;

namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Random dither filter.
    /// </summary>
    [Filter]
    public sealed class RandomDitherFilter : PerPixelFilterBase
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

        private Random rnd = null;
        private float intervalSize;

        /// <summary>
        /// Gets called when filter starts applying.
        /// </summary>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            rnd = new Random(Seed);
            intervalSize = 255f / (levels - 1);
        }

        /// <inheritdoc/>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            float floor, ceil;
            float nextRnd = rnd.NextSingle();

            floor = System.MathF.Floor(*r / intervalSize) * intervalSize;
            ceil = floor + intervalSize;
            *r = ((floor + nextRnd * intervalSize > *r) ? floor : ceil).ClampToByte();
            
            floor = System.MathF.Floor(*g / intervalSize) * intervalSize;
            ceil = floor + intervalSize;
            *g = ((floor + nextRnd * intervalSize > *g) ? floor : ceil).ClampToByte();
            
            floor = System.MathF.Floor(*b / intervalSize) * intervalSize;
            ceil = floor + intervalSize;
            *b = ((floor + nextRnd * intervalSize > *b) ? floor : ceil).ClampToByte();
        }

        /// <summary>
        /// Gets called when filter finishes applying.
        /// </summary>
        protected override void ApplyEnd()
        {
            rnd = null;
            base.ApplyEnd();
        }
    }
}
