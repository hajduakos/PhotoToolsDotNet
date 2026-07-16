using FilterLib.Util;

namespace FilterLib.Filters.Color
{
    [Filter("Threshold filter that maps each pixel to pure black or white based on a given (and fixed) threshold.")]
    public sealed class TresholdFilter : PerPixelFilterBase
    {
        /// <summary>
        /// Threshold value [0;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(255)]
        public int Treshold
        {
            get;
            set { field = value.ClampToByte(); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="treshold">Threshold value [0:255]</param>
        public TresholdFilter(int treshold = 127) => Treshold = treshold;

        // Cache
        private byte[] map;

        /// <inheritdoc/>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            // Fill cache
            map = new byte[256];
            for (int i = 0; i < 256; ++i) map[i] = (byte)(i < Treshold ? 0 : 255);
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
