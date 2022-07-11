using Conv3x3 = FilterLib.Util.Conv3x3;
using ConvolutionFilter = FilterLib.Filters.Other.ConvolutionFilter;
using IReporter = FilterLib.Reporting.IReporter;

namespace FilterLib.Filters.Edges
{
    /// <summary>
    /// Edge detection filter using a 3x3 convolution.
    /// </summary>
    [Filter]
    public sealed class EdgeDetectionFilter : FilterInPlaceBase
    {
        private readonly ConvolutionFilter conv = new(new Conv3x3(-1, 0, -1, 0, 4, 0, -1, 0, -1, 1, 127));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
