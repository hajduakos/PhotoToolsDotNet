using FilterLib.Util;
using MathF = System.MathF;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Posterize filter that reduces the levels of each component.
    /// </summary>
    [Filter]
    public sealed class PosterizeFilter : PerComponentFilterBase
    {
        private int levels;

        /// <summary>
        /// Number of levels [2:256]
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
        /// Constructor with levels parameter.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public PosterizeFilter(int levels = 256)
        {
            this.Levels = levels;
        }

        /// <summary>
        /// Map a single (R/G/B) component.
        /// </summary>
        /// <param name="comp">Input value</param>
        /// <returns>Output value by applying the filter</returns>
        protected override byte MapComponent(byte comp)
        {
            float div = 255f / (levels - 1); // Divisor
            float divRecip = 1f / div; // Reciprocial of divisor
            return (byte)MathF.Round(MathF.Round(comp * divRecip) * div);
        }
    }
}
