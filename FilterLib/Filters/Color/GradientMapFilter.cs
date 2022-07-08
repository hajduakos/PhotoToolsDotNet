using FilterLib.Util;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Map each color to a value in a given gradient based on its luminance.
    /// </summary>
    [Filter]
    public sealed class GradientMapFilter : PerPixelFilterBase
    {
        /// <summary>
        /// Gradient.
        /// </summary>
        [FilterParam]
        public Gradient GradientMap { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="gradientMap">Gradient</param>
        public GradientMapFilter(Gradient gradientMap) => GradientMap = gradientMap;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GradientMapFilter() : this(new Gradient(new RGB(0, 0, 0), new RGB(255, 255, 255))) { }

        private RGB[] map;

        /// <inheritdoc/>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            map = GradientMap.CreateMap256();
        }

        /// <inheritdoc/>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            System.Diagnostics.Debug.Assert(map != null);
            System.Diagnostics.Debug.Assert(map.Length == 256);
            byte lum = (byte)RGB.GetLuminance(*r, *g, *b);
            *r = map[lum].R;
            *g = map[lum].G;
            *b = map[lum].B;
        }

        /// <inheritdoc/>
        protected override void ApplyEnd()
        {
            map = null;
            base.ApplyEnd();
        }
    }
}
