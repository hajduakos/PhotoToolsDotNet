using FilterLib.Util;

namespace FilterLib.Adjustments
{
    /// <summary>
    /// Color adjustment in the HSL (hue, saturation, lightness) color space.
    /// </summary>
    [Filter]
    public sealed class ColorHSLFilter : PerPixelFilterBase
    {
        private int hue, saturation, lightness;

        /// <summary>
        /// Hue adjustment [-180;180]
        /// </summary>
        [FilterParam]
        [FilterParamMin(-180)]
        [FilterParamMax(180)]
        public int Hue
        {
            get { return hue; }
            set { hue = value.Clamp(-180, 180); }
        }

        /// <summary>
        /// Saturation adjustment [-100;100]
        /// </summary>
        [FilterParam]
        [FilterParamMin(-100)]
        [FilterParamMax(100)]
        public int Saturation
        {
            get { return saturation; }
            set { saturation = value.Clamp(-100, 100); }
        }

        /// <summary>
        /// Lightness adjustment [-100;100]
        /// </summary>
        [FilterParam]
        [FilterParamMin(-100)]
        [FilterParamMax(100)]
        public int Lightness
        {
            get { return lightness; }
            set { lightness = value.Clamp(-100, 100); }
        }

        /// <summary>
        /// Constructor with hue, saturation and lightness parameters </summary>
        /// <param name="hue">Hue [-180;180]</param>
        /// <param name="saturation">Saturation [-100;100]</param>
        /// <param name="lightness">Lightness [-100;100]</param>
        public ColorHSLFilter(int hue = 0, int saturation = 0, int lightness = 0)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Lightness = lightness;
        }

        int[] satMap = null;
        int[] lightMap = null;
        int[] hueMap = null;

        /// <summary>
        /// Gets called when filter starts applying.
        /// </summary>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            satMap = new int[101];
            lightMap = new int[101];
            hueMap = new int[361];
            // Lightness and saturation
            for (int x = 0; x <= 100; ++x)
            {
                satMap[x] = (x + saturation).Clamp(0, 100);
                lightMap[x] = (x + lightness).Clamp(0, 100);
            }
            // Hue
            for (int x = 0; x <= 360; ++x)
            {
                hueMap[x] = x + hue;
                if (hueMap[x] > 360) hueMap[x] %= 360;
                else if (hueMap[x] < 0) hueMap[x] += 360;
            }
        }

        /// <summary>
        /// Gets called for each pixel independently.
        /// </summary>
        /// <param name="r">Pointer to red value</param>
        /// <param name="g">Pointer to green value</param>
        /// <param name="b">Pointer to blue value</param>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            // Convert RGB to HSL
            HSL hsl = new RGB(*r, *g, *b).ToHSL();
            // Adjust in HSL space
            hsl = new HSL(hueMap[hsl.H], satMap[hsl.S], lightMap[hsl.L]);
            // Convert back to RGB
            RGB rgb = hsl.ToRGB();
            *r = (byte)rgb.R;
            *g = (byte)rgb.G;
            *b = (byte)rgb.B;
        }

        /// <summary>
        /// Gets called when filter finishes applying.
        /// </summary>
        protected override void ApplyEnd()
        {
            satMap = hueMap = lightMap = null;
            base.ApplyEnd();
        }
    }
}
