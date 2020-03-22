using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Level adjustment filter.
    /// </summary>
    [Filter]
    public sealed class LevelsFilter : PerComponentFilterBase
    {
        private int dark, light;

        /// <summary>
        /// Dark level property [0;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(255)]
        public int Dark
        {
            get { return dark; }
            set { dark = value.Clamp(0, 255); }
        }

        /// <summary>
        /// Light level property [0;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(255)]
        public int Light
        {
            get { return light; }
            set { light = value.Clamp(0, 255); }
        }

        /// <summary>
        /// Constructor with light and dark level parameters.
        /// </summary>
        /// <param name="dark">Dark level [0:255]</param>
        /// <param name="light">Light level [0:255]</param>
        public LevelsFilter(int dark = 0, int light = 255)
        {
            this.Dark = dark;
            this.Light = light;
        }

        /// <summary>
        /// Map a single (R/G/B) component.
        /// </summary>
        /// <param name="comp">Input value</param>
        /// <returns>Output value by applying the filter</returns>
        protected override byte MapComponent(byte comp)
        {
            if (comp < dark) return 0; // Black under dark level
            if (comp > light) return 255; // White above light level
            return (byte)((comp - dark) / (float)(light - dark) * 255); // Linear transition between
        }
    }
}
