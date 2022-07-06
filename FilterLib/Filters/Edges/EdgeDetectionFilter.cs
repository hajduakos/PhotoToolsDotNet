using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Edges
{
    /// <summary>
    /// Edge detection filter.
    /// </summary>
    [Filter]
    public sealed class EdgeDetectionFilter : FilterInPlaceBase
    {
        private readonly Other.ConvolutionFilter conv =
            new(new Conv3x3(-1, 0, -1, 0, 4, 0, -1, 0, -1, 1, 127));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
