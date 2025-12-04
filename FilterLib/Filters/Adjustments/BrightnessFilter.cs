using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    [Filter("Increase/decrease each component of each pixel by a fixed amount.")]
    public sealed class BrightnessFilter : PerComponentFilterBase
    {
        private int brightness;

        /// <summary>
        /// Brightness adjustment amount [-255;255]
        /// </summary>
        [FilterParam]
        [FilterParamMin(-255)]
        [FilterParamMax(255)]
        public int Brightness
        {
            get { return brightness; }
            set { brightness = value.Clamp(-255, 255); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="brightness">Brightness adjustment amount [-255;255]</param>
        public BrightnessFilter(int brightness = 0) => Brightness = brightness;

        /// <inheritdoc/>
        protected override byte MapComponent(byte comp) => (comp + brightness).ClampToByte();
    }
}
