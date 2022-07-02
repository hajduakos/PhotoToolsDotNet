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
            set { treshold = value.ClampToByte(); }
        }

        /// <summary>
        /// Constructor with treshold value.
        /// </summary>
        /// <param name="treshold">Treshold value [0:255]</param>
        public TresholdFilter(int treshold = 127) => Treshold = treshold;

        private byte[] map;

        /// <inheritdoc/>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            map = new byte[256];
            for (int x = 0; x < 256; ++x) map[x] = (byte)(x < treshold ? 0 : 255);
        }

        /// <inheritdoc/>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            System.Diagnostics.Debug.Assert(map != null);
            int luminance = (int)(.299f * (*r) + .587f * (*g) + .114f * (*b));
            *r = *g = *b = map[luminance];
        }

        /// <inheritdoc/>
        protected override void ApplyEnd()
        {
            map = null;
            base.ApplyEnd();
        }
    }
}
