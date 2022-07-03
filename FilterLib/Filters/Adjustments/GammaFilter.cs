using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Adjust gamma with an exponential transformation over pixel values.
    /// </summary>
    [Filter]
    public sealed class GammaFilter : PerComponentFilterBase
    {
        private float gamma;

        /// <summary>
        /// Gamma adjustment amount ]0;...]
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0.001f)]
        public float Gamma
        {
            get { return gamma; }
            set { gamma = System.MathF.Max(value, 0.001f); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="gamma">Gamma adjustment amount ]0;...]</param>
        public GammaFilter(float gamma = 1f) => Gamma = gamma;

        /// <inheritdoc/>
        protected override byte MapComponent(byte comp) => (255 * System.MathF.Pow(comp / 255f, 1f / gamma)).ClampToByte();
    }
}
