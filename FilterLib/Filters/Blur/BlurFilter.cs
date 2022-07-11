using Conv3x3 = FilterLib.Util.Conv3x3;
using ConvolutionFilter = FilterLib.Filters.Other.ConvolutionFilter;
using IReporter = FilterLib.Reporting.IReporter;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Apply a small amount of blur with a 3x3 convolution.
    /// </summary>
    [Filter]
    public class BlurFilter : FilterInPlaceBase
    {
        private readonly ConvolutionFilter conv = new(new Conv3x3(1, 1, 1, 1, 8, 1, 1, 1, 1, 16, 0));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
