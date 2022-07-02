using FilterLib.Util;
using System;

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
            set { dark = value.ClampToByte(); }
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
            set { light = value.ClampToByte(); }
        }

        /// <summary>
        /// Constructor with light and dark level parameters.
        /// </summary>
        /// <param name="dark">Dark level [0:255]</param>
        /// <param name="light">Light level [0:255]</param>
        public LevelsFilter(int dark = 0, int light = 255)
        {
            Dark = dark;
            Light = light;
        }

        /// <inheritdoc/>
        protected override byte MapComponent(byte comp)
        {
            if (light <= dark) throw new ArgumentException("Light must be greater than dark.");
            if (comp < dark) return 0; // Black under dark level
            if (comp > light) return 255; // White above light level
            return (byte)((comp - dark) / (float)(light - dark) * 255); // Linear transition between
        }
    }
}
