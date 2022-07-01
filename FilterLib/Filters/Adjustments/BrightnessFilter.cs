using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Brightness adjustment filter.
    /// </summary>
    [Filter]
    public sealed class BrightnessFilter : PerComponentFilterBase
    {
        private int brightness;

        /// <summary>
        /// Brightness adjustment property [-255;255]
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
        /// Constructor with brightness parameter.
        /// </summary>
        /// <param name="brightness">Brightness adjustment value [-255;255]</param>
        public BrightnessFilter(int brightness = 0) => Brightness = brightness;

        /// <inheritdoc/>
        protected override byte MapComponent(byte comp) => (byte)(comp + brightness).Clamp(0, 255);
    }
}
