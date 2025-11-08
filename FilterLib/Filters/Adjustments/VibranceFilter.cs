using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Adjust vibrance, which is essentially an adaptive saturation.
    /// </summary>
    [Filter]
    public sealed class VibranceFilter : PerPixelFilterBase
    {
        private int vibrance;

        /// <summary>
        /// Vibrance adjustment [-100;100]
        /// </summary>
        [FilterParam]
        [FilterParamMin(-100)]
        [FilterParamMax(100)]
        public int Vibrance
        {
            get { return vibrance; }
            set { vibrance = value.Clamp(-100, 100); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="vibrance">Vibrance adjustment [-100;100]</param>
        public VibranceFilter(int vibrance = 0)
        {
            Vibrance = vibrance;
        }

        // Cache
        private int[] vibMap;

        /// <inheritdoc/>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            // Calculate all possible values and save in cache
            vibMap = new int[101];
            for (int x = 0; x <= 100; ++x)
            {
                // Smaller multiplier for already saturated pixels
                float mult = (1 - x / 100f) * (1 - x / 100f);
                vibMap[x] = System.Convert.ToInt32(x + vibrance * mult).Clamp(0, 100);
            }
        }

        /// <inheritdoc/>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            System.Diagnostics.Debug.Assert(vibMap != null);
            // Convert to HSL
            HSL hsl = new RGB(*r, *g, *b).ToHSL();
            // Adjust in HSL space using cache
            hsl = new HSL(hsl.H, vibMap[hsl.S], hsl.L);
            // Convert back to RGB
            RGB rgb = hsl.ToRGB();
            *r = rgb.R;
            *g = rgb.G;
            *b = rgb.B;
        }

        /// <inheritdoc/>
        protected override void ApplyEnd()
        {
            vibMap = null;
            base.ApplyEnd();
        }
    }
}
