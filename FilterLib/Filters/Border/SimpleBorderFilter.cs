using FilterLib.Util;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Simple border filter.
    /// </summary>
    [Filter]
    public sealed class SimpleBorderFilter : BorderFilterBase
    {
        /// <summary>
        /// Border color.
        /// </summary>
        [FilterParam]
        public RGB Color { get; set; }

        public SimpleBorderFilter() :
            this(Size.Absolute(0), Size.Absolute(0), new RGB(0, 0, 0), BorderPosition.Inside, AntiAliasQuality.Medium) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Border width</param>
        /// <param name="radius">Border radius</param>
        /// <param name="color">Border color</param>
        /// <param name="position">Border position</param>
        /// <param name="antiAlias">Quality of anti-aliasing the rounded corners</param>
        public SimpleBorderFilter(Size width, Size radius, RGB color, BorderPosition position, AntiAliasQuality antiAlias)
            : base(width, radius, position, antiAlias) => Color = color;

        protected override (byte, byte, byte) GetBorderAt(int x, int y) => (Color.R, Color.G, Color.B);
    }
}
