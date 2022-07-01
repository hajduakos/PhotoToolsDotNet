using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Contrast adjustment filter.
    /// </summary>
    [Filter]
    public sealed class ContrastFilter : PerComponentFilterBase
    {
        private int contrast;

        /// <summary>
        /// Contrast adjustment property [-100;100].
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
        /// Constructor with contrast adjustment parameter.
        /// </summary>
        /// <param name="contrast">Contrast adjustment value [-100;100]</param>
        public ContrastFilter(int contrast = 0)
        {
            Contrast = contrast;
        }

        /// <summary>
        /// Map a single (R/G/B) component.
        /// </summary>
        /// <param name="comp">Input value</param>
        /// <returns>Output value by applying the filter</returns>
        protected override byte MapComponent(byte comp)
        {
            float normalized = (100 + contrast) / 100f;
            return (byte)(((comp / 255f - .5f) * normalized + .5f) * 255f).Clamp(0, 255);
        }
    }
}
