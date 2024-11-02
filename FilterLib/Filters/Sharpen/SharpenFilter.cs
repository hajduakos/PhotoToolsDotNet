using ConvolutionFilter = FilterLib.Filters.Other.ConvolutionFilter;
using ConvolutionMatrix = FilterLib.Util.ConvolutionMatrix;
using IReporter = FilterLib.Reporting.IReporter;

namespace FilterLib.Filters.Sharpen
{
    /// <summary>
    /// Sharpen image using 3x3 convolution.
    /// </summary>
    [Filter]
    public sealed class SharpenFilter : FilterInPlaceBase
    {
        private readonly ConvolutionFilter conv = new(new ConvolutionMatrix(new int[,] { { 0, -2, 0 }, { -2, 11, -2 }, { 0, -2, 0 } }, 3, 0));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
