using FilterLib.Reporting;
using FilterLib.Util;
using System.Drawing;

namespace FilterLib.Filters.Edges
{
    /// <summary>
    /// Emboss filter.
    /// </summary>
    [Filter]
    public sealed class EmbossFilter : FilterInPlaceBase
    {
        private readonly Other.ConvolutionFilter conv =
            new(new Conv3x3(1, 1, 1, 0, 0, 0, -1, -1, -1, 1, 127));

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
