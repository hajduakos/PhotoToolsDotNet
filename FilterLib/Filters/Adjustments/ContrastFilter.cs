using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    [Filter("Adjust contrast by darkening tones below the midpoint and brightening tones above.")]
    public sealed class ContrastFilter : PerComponentFilterBase
    {
        /// <summary>
        /// Contrast adjustment amount [-100;100].
        /// </summary>
        [FilterParam]
        [FilterParamMin(-100)]
        [FilterParamMax(100)]
        public int Contrast
        {
            get;
            set { field = value.Clamp(-100, 100); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contrast">Contrast adjustment amount [-100;100]</param>
        public ContrastFilter(int contrast = 0) => Contrast = contrast;

        /// <inheritdoc/>
        protected override byte MapComponent(byte comp)
        {
            float normalized = (100 + Contrast) / 100f;
            return (((comp / 255f - .5f) * normalized + .5f) * 255f).ClampToByte();
        }
    }
}
