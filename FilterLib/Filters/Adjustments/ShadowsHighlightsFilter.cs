using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    [Filter("Brighten darker tones and darken lighter tones.")]
    public sealed class ShadowsHighlightsFilter : PerComponentFilterBase
    {
        /// <summary>
        /// Brighten shadows amount [0;100].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(100)]
        public int Brighten
        {
            get;
            set { field = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Darken highlights amount [0;100].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(100)]
        public int Darken
        {
            get;
            set { field = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="brighten">Brighten shadows amount [0;100]</param>
        /// <param name="darken">Darken highlights amount [0;100]</param>
        public ShadowsHighlightsFilter(int brighten = 0, int darken = 0)
        {
            Brighten = brighten;
            Darken = darken;
        }

        /// <inheritdoc/>
        protected override byte MapComponent(byte comp)
        {
            if (Brighten == 0 && Darken == 0) return comp;

            // Mulitply pixel with itself (darkening)
            float mult = comp * comp / 255f;
            // Blend with original value
            float darkPct = Darken / 100f;
            mult = comp * (1 - darkPct) + mult * darkPct;

            // Screen pixel with itself (brightening)
            float screen = (255 - comp) * comp / 255f + comp;
            // Blend with original value
            float brightPct = Brighten / 100f;
            screen = comp * (1 - brightPct) + screen * brightPct;

            // Blend darkened and brightened pixels together
            float darkRatio = Darken / (float)(Brighten + Darken);
            float brightRatio = 1 - darkRatio;
            return (darkRatio * mult + brightRatio * screen).ClampToByte();
        }
    }
}
