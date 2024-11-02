using ConvolutionMatrix = FilterLib.Util.ConvolutionMatrix;
using ConvolutionFilter = FilterLib.Filters.Other.ConvolutionFilter;
using IReporter = FilterLib.Reporting.IReporter;

namespace FilterLib.Filters.Edges
{
    /// <summary>
    /// Emboss filter using a 3x3 convolution.
    /// </summary>
    [Filter]
    public sealed class EmbossFilter : FilterInPlaceBase
    {
        private readonly ConvolutionFilter conv = new(new ConvolutionMatrix(new int[,] { { 1, 0, -1 }, { 1, 0, -1 }, { 1, 0, -1 } }, 1, 127));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
