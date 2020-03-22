using FilterLib.Util;

namespace FilterLib.Adjustments
{
    /// <summary>
    /// Gamma adjustment filter.
    /// </summary>
    public sealed class GammaFilter : PerComponentFilterBase
    {
        private float gamma;

        /// <summary>
        /// Gamma adjustment property ]0;...]
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0.001f)]
        public float Gamma
        {
            get { return gamma; }
            set { gamma = value <= 0.001f ? 0.001f : value;  }
        }

        /// <summary>
        /// Constructor with gamma adjustment parameter.
        /// </summary>
        /// <param name="gamma">Gamma adjustment value ]0;...]</param>
        public GammaFilter(float gamma = 1f) => this.Gamma = gamma;

        /// <summary>
        /// Map a single (R/G/B) component.
        /// </summary>
        /// <param name="comp">Input value</param>
        /// <returns>Output value by applying the filter</returns>
        protected override byte MapComponent(byte comp) => (byte)(255 * System.Math.Pow(comp / 255f, 1f / gamma)).Clamp(0, 255);
    }
}
