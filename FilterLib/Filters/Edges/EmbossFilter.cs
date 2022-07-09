using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Edges
{
    /// <summary>
    /// Emboss filter using a 3x3 convolution.
    /// </summary>
    [Filter]
    public sealed class EmbossFilter : FilterInPlaceBase
    {
        private readonly Other.ConvolutionFilter conv =
            new(new Conv3x3(1, 1, 1, 0, 0, 0, -1, -1, -1, 1, 127));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
