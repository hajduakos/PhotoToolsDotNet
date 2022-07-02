using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Adjust brightness by increasing/decreasing the value of each component
    /// with a fixed amount.
    /// </summary>
    [Filter]
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
