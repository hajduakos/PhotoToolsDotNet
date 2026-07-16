using ConvolutionFilter = FilterLib.Filters.Other.ConvolutionFilter;
using ConvolutionMatrix = FilterLib.Util.ConvolutionMatrix;
using IReporter = FilterLib.Reporting.IReporter;

namespace FilterLib.Filters.Sharpen
{
    [Filter("Mean removal filter using a 3x3 convolution.")]
    public sealed class MeanRemovalFilter : FilterInPlaceBase
    {
        private readonly ConvolutionFilter conv = new(new ConvolutionMatrix(new int[,] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } }, 1, 0));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
