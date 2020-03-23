using FilterLib.Util;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Treshold filter.
    /// </summary>
    [Filter]
    public sealed class TresholdFilter : PerPixelFilterBase
    {
        private int treshold;

        /// <summary>
        /// Treshold value [0;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(255)]
        public int Treshold
        {
            get { return treshold; }
            set { treshold = value.Clamp(0, 255); }
        }

        /// <summary>
        /// Constructor with treshold value.
        /// </summary>
        /// <param name="treshold">Treshold value [0:255]</param>
        public TresholdFilter(int treshold = 127)
        {
            this.Treshold = treshold;
        }

        private byte[] map;

        /// <summary>
        /// Gets called when filter starts applying.
        /// </summary>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            map = new byte[256];
            for (int x = 0; x < 256; ++x) map[x] = (byte)(x < treshold ? 0 : 255);
        }

        /// <summary>
        /// Gets called for each pixel independently.
        /// </summary>
        /// <param name="r">Pointer to red value</param>
        /// <param name="g">Pointer to green value</param>
        /// <param name="b">Pointer to blue value</param>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            int luminance = (int)(.299f * (*r) + .587f * (*g) + .114f * (*b));
            *r = *g = *b = map[luminance];
        }

        /// <summary>
        /// Gets called when filter finishes applying.
        /// </summary>
        protected override void ApplyEnd()
        {
            map = null;
            base.ApplyEnd();
        }
    }
}
