using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Adjust color in the hue-saturation-lightness (HSL) space.
    /// </summary>
    [Filter("Adjust the hue, saturation and lightness of each pixel.")]
    public sealed class ColorHSLFilter : PerPixelFilterBase
    {
        /// <summary>
        /// Hue adjustment [-180;180]
        /// </summary>
        [FilterParam]
        [FilterParamMin(-180)]
        [FilterParamMax(180)]
        public int Hue
        {
            get ;
            set { field = value.Clamp(-180, 180); }
        }

        /// <summary>
        /// Saturation adjustment [-100;100]
        /// </summary>
        [FilterParam]
        [FilterParamMin(-100)]
        [FilterParamMax(100)]
        public int Saturation
        {
            get;
            set { field = value.Clamp(-100, 100); }
        }

        /// <summary>
        /// Lightness adjustment [-100;100]
        /// </summary>
        [FilterParam]
        [FilterParamMin(-100)]
        [FilterParamMax(100)]
        public int Lightness
        {
            get;
            set { field = value.Clamp(-100, 100); }
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
        private int[] satMap;
        private int[] lightMap;
        private int[] hueMap;

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
                satMap[x] = (x + Saturation).Clamp(0, 100);
                lightMap[x] = (x + Lightness).Clamp(0, 100);
            }
            for (int x = 0; x <= 360; ++x)
            {
                hueMap[x] = x + Hue;
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
