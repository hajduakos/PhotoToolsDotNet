using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Sharpen
{
    /// <summary>
    /// Mean removal filter.
    /// </summary>
    [Filter]
    public sealed class MeanRemovalFilter : FilterInPlaceBase
    {
        private readonly Other.ConvolutionFilter conv =
               new(new Conv3x3(-1, -1, -1, -1, 9, -1, -1, -1, -1, 1, 0));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
