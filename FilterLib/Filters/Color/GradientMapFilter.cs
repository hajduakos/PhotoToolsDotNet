using FilterLib.Util;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Gradient map filter.
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
        public GradientMapFilter(Gradient gradientMap)
        {
            GradientMap = gradientMap;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GradientMapFilter() : this(new Gradient(new RGB(0, 0, 0), new RGB(255, 255, 255))) { }

        private RGB[] map;

        /// <summary>
        /// Gets called when filter starts applying.
        /// </summary>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            map = GradientMap.CreateMap256();
        }

        /// <summary>
        /// Gets called for each pixel independently.
        /// </summary>
        /// <param name="r">Pointer to red value</param>
        /// <param name="g">Pointer to green value</param>
        /// <param name="b">Pointer to blue value</param>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            int lum = (int)(.299f * (*r) + .587f * (*g) + .114f * (*b));
            *r = (byte)map[lum].R;
            *g = (byte)map[lum].G;
            *b = (byte)map[lum].B;
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
