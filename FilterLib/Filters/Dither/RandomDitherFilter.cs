using System;
using FilterLib.Util;

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
            this.Levels = levels;
            this.Seed = seed;
        }

        Random rnd = null;
        float intervalSize;

        /// <summary>
        /// Gets called when filter starts applying.
        /// </summary>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            rnd = new Random(Seed);
            intervalSize = 255f / (levels - 1);
        }

        /// <summary>
        /// Gets called for each pixel independently.
        /// </summary>
        /// <param name="r">Pointer to red value</param>
        /// <param name="g">Pointer to green value</param>
        /// <param name="b">Pointer to blue value</param>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            float roundedColor, nextRnd;

            nextRnd = (float)rnd.NextDouble();

            roundedColor = System.MathF.Floor(*r / intervalSize) * intervalSize;
            *r = (byte)((roundedColor + nextRnd * intervalSize > *r) ? roundedColor : (roundedColor + intervalSize));
            
            roundedColor = System.MathF.Floor(*g / intervalSize) * intervalSize;
            *g = (byte)((roundedColor + nextRnd * intervalSize > *g) ? roundedColor : (roundedColor + intervalSize));
            
            roundedColor = System.MathF.Floor(*b / intervalSize) * intervalSize;
            *b = (byte)((roundedColor + nextRnd * intervalSize > *b) ? roundedColor : (roundedColor + intervalSize));
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
