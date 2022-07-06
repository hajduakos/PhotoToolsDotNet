using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Blur filter.
    /// </summary>
    [Filter]
    public class BlurFilter : FilterInPlaceBase
    {
        private readonly Other.ConvolutionFilter conv =
            new(new Conv3x3(1, 1, 1, 1, 8, 1, 1, 1, 1, 16, 0));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
