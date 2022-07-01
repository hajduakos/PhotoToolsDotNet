using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Shadows and highlights filter.
    /// </summary>
    [Filter]
    public sealed class ShadowsHighlightsFilter : PerComponentFilterBase
    {
        private int darken, brighten;

        /// <summary>
        /// Brighten shadows [0;100].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(100)]
        public int Brighten
        {
            get { return brighten; }
            set { brighten = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Darken highlights [0;100].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(100)]
        public int Darken
        {
            get { return darken; }
            set { darken = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="brighten">Brighten shadows [0;100]</param>
        /// <param name="darken">Darken highlights [0;100]</param>
        public ShadowsHighlightsFilter(int brighten = 0, int darken = 0)
        {
            Brighten = brighten;
            Darken = darken;
        }

        /// <inheritdoc/>
        protected override byte MapComponent(byte comp)
        {
            if (brighten == 0 && darken == 0) return comp;

            float darkOp1 = darken / 100f; // Percent of darkening
            float darkOp0 = 1 - darkOp1;
            float brightOp1 = brighten / 100f; // Percent of brightening
            float brightOp0 = 1 - brightOp1;
            // Ratio of darken to brighten
            float darkRatio = darken / (float)(brighten + darken);
            float brightRatio = 1 - darkRatio;

            // Mulitply pixel with itself (darkening)
            float mult = comp * comp / 255f;
            // Blend with original value
            mult = comp * darkOp0 + mult * darkOp1;

            // Screen pixel with itself (brightening)
            float screen = (255 - comp) * comp / 255f + comp;
            // Blend with original value
            screen = comp * brightOp0 + screen * brightOp1;

            // Blend darkened and brightened pixels together
            return (byte)(darkRatio * mult + brightRatio * screen);
        }
    }
}
