using Conv3x3 = FilterLib.Util.Conv3x3;
using ConvolutionFilter = FilterLib.Filters.Other.ConvolutionFilter;
using IReporter = FilterLib.Reporting.IReporter;

namespace FilterLib.Filters.Sharpen
{
    /// <summary>
    /// Sharpen image using 3x3 convolution.
    /// </summary>
    [Filter]
    public sealed class SharpenFilter : FilterInPlaceBase
    {
        private readonly ConvolutionFilter conv = new(new Conv3x3(0, -2, 0, -2, 11, -2, 0, -2, 0, 3, 0));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
