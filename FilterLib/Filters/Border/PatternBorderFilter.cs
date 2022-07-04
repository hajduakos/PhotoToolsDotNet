using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Pattern border filter.
    /// </summary>
    [Filter]
    public sealed class PatternBorderFilter : BorderFilterBase
    {
        /// <summary>
        /// Border pattern.
        /// </summary>
        [FilterParam]
        public Bitmap Pattern { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PatternBorderFilter() :
            this(Util.Size.Absolute(0), Util.Size.Absolute(0), new Bitmap(1, 1), BorderPosition.Inside, AntiAliasQuality.Medium)
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Border width</param>
        /// <param name="radius">Border radius</param>
        /// <param name="pattern">Border pattern</param>
        /// <param name="position">Border position</param>
        public PatternBorderFilter(Util.Size width, Util.Size radius, Bitmap pattern, BorderPosition position, AntiAliasQuality antiAlias)
            : base(width, radius, position, antiAlias) => Pattern = pattern;

        protected override (byte, byte, byte) GetBorderAt(int x, int y)
        {
            System.Drawing.Color c = Pattern.GetPixel(x % Pattern.Width, y % Pattern.Height);
            return (c.R, c.G, c.B);
        }

        /// <inheritdoc/>
        public override string ParamToString(object param)
        {
            if (param is Bitmap)
            {
                Bitmap bmp = param as Bitmap;
                return $"Bitmap({bmp.Width}x{bmp.Height})";
            }
            return base.ParamToString(param);
        }
    }
}
