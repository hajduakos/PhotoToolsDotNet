using FilterLib.Util;

namespace FilterLib.Adjustments
{
    public sealed class BrightnessFilter : PerComponentFilterBase
    {
        private int brightness;

        public int Brightness
        {
            get { return brightness; }
            set { brightness = value.Clamp(-255, 255); }
        }

        public BrightnessFilter(int brightness = 0)
        {
            this.Brightness = brightness;
        }

        protected override byte MapComponent(byte comp)
        {
            return (byte)(comp + brightness).Clamp(0, 255);
        }
    }
}
