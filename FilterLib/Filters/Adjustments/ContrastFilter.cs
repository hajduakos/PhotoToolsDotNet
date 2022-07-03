using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Adjust contrast by darkening/brightening tones below/above the midpoint.
    /// </summary>
    [Filter]
    public sealed class ContrastFilter : PerComponentFilterBase
    {
        private int contrast;

        /// <summary>
        /// Contrast adjustment amount [-100;100].
        /// </summary>
        [FilterParam]
        [FilterParamMin(-100)]
        [FilterParamMax(100)]
        public int Contrast
        {
            get { return contrast; }
            set { contrast = value.Clamp(-100, 100); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contrast">Contrast adjustment amount [-100;100]</param>
        public ContrastFilter(int contrast = 0) => Contrast = contrast;

        /// <inheritdoc/>
        protected override byte MapComponent(byte comp)
        {
            float normalized = (100 + contrast) / 100f;
            return (((comp / 255f - .5f) * normalized + .5f) * 255f).ClampToByte();
        }
    }
}
