using FilterLib.Util;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Add a border using an other image.
    /// </summary>
    [Filter]
    public sealed class PatternBorderFilter : BorderFilterBase
    {
        /// <summary>
        /// Border pattern.
        /// </summary>
        [FilterParam]
        public Image Pattern { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PatternBorderFilter() :
            this(Size.Absolute(0), Size.Absolute(0), new Image(1, 1), BorderPosition.Inside, AntiAliasQuality.Medium)
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Border width</param>
        /// <param name="radius">Border radius</param>
        /// <param name="pattern">Border pattern</param>
        /// <param name="position">Border position</param>
        public PatternBorderFilter(Size width, Size radius, Image pattern, BorderPosition position, AntiAliasQuality antiAlias)
            : base(width, radius, position, antiAlias) => Pattern = pattern;

        protected override (byte, byte, byte) GetBorderAt(int x, int y)
        {
            int x1 = x % Pattern.Width;
            int y1 = y % Pattern.Height;
            return (Pattern[x1, y1, 0], Pattern[x1, y1, 1], Pattern[x1, y1, 2]);
        }

        /// <inheritdoc/>
        public override string ParamToString(object param)
        {
            if (param is Image img) return $"Image({img.Width}x{img.Height})";
            return base.ParamToString(param);
        }
    }
}
