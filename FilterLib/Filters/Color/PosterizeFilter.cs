using FilterLib.Util;
using System.IO;
using MathF = System.MathF;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Posterize image by reducing the number of levels for each component.
    /// </summary>
    [Filter]
    public sealed class PosterizeFilter : PerComponentFilterBase
    {
        private int levels;
        private float div;

        /// <summary>
        /// Number of levels [2:256].
        /// </summary>
        [FilterParam]
        [FilterParamMin(2)]
        [FilterParamMax(256)]
        public int Levels
        {
            get { return levels; }
            set
            {
                levels = value.Clamp(2, 256);
                div = 255f / (levels - 1);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public PosterizeFilter(int levels = 256) => Levels = levels;

        /// <inheritdoc/>
        protected override byte MapComponent(byte comp) => (byte)MathF.Round(MathF.Round(comp / div) * div);
    }
}
