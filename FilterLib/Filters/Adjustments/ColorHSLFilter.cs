using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Adjust color in the hue-saturation-lightness (HSL) space.
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
        /// Constructor.
        /// </summary>
        /// <param name="hue">Hue adjustment [-180;180]</param>
        /// <param name="saturation">Saturation adjustment [-100;100]</param>
        /// <param name="lightness">Lightness adjustment [-100;100]</param>
        public ColorHSLFilter(int hue = 0, int saturation = 0, int lightness = 0)
        {
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }

        // Caches
        private int[] satMap = null;
        private int[] lightMap = null;
        private int[] hueMap = null;

        /// <inheritdoc/>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            // Calculate all possible values and save in cache
            satMap = new int[101];
            lightMap = new int[101];
            hueMap = new int[361];
            for (int x = 0; x <= 100; ++x)
            {
                satMap[x] = (x + saturation).Clamp(0, 100);
                lightMap[x] = (x + lightness).Clamp(0, 100);
            }
            for (int x = 0; x <= 360; ++x)
            {
                hueMap[x] = x + hue;
                if (hueMap[x] > 360) hueMap[x] %= 360;
                else if (hueMap[x] < 0) hueMap[x] += 360;
            }
        }

        /// <inheritdoc/>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            System.Diagnostics.Debug.Assert(hueMap != null);
            System.Diagnostics.Debug.Assert(satMap != null);
            System.Diagnostics.Debug.Assert(lightMap != null);
            // Convert to HSL
            HSL hsl = new RGB(*r, *g, *b).ToHSL();
            // Adjust in HSL space using cache
            hsl = new HSL(hueMap[hsl.H], satMap[hsl.S], lightMap[hsl.L]);
            // Convert back to RGB
            RGB rgb = hsl.ToRGB();
            *r = rgb.R;
            *g = rgb.G;
            *b = rgb.B;
        }

        /// <inheritdoc/>
        protected override void ApplyEnd()
        {
            satMap = hueMap = lightMap = null;
            base.ApplyEnd();
        }
    }
}
