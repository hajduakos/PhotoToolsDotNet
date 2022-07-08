using FilterLib.Util;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Treshold filter that maps each pixel to pure black or white based on
    /// a given (and fixed) treshold.
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
            set { treshold = value.ClampToByte(); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="treshold">Treshold value [0:255]</param>
        public TresholdFilter(int treshold = 127) => Treshold = treshold;

        // Cache
        private byte[] map;

        /// <inheritdoc/>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            // Fill cache
            map = new byte[256];
            for (int i = 0; i < 256; ++i) map[i] = (byte)(i < treshold ? 0 : 255);
        }

        /// <inheritdoc/>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            // Use cache
            System.Diagnostics.Debug.Assert(map != null);
            System.Diagnostics.Debug.Assert(map.Length == 256);
            *r = *g = *b = map[(byte)RGB.GetLuminance(*r, *g, *b)];
        }

        /// <inheritdoc/>
        protected override void ApplyEnd()
        {
            map = null;
            base.ApplyEnd();
        }
    }
}
