using ConvolutionFilter = FilterLib.Filters.Other.ConvolutionFilter;
using ConvolutionMatrix = FilterLib.Util.ConvolutionMatrix;
using IReporter = FilterLib.Reporting.IReporter;

namespace FilterLib.Filters.Edges;

[Filter("Highlight edges by detecting sharp changes in brightness.")]
public sealed class EdgeDetectionFilter : FilterInPlaceBase
{
    private readonly ConvolutionFilter conv = new(new ConvolutionMatrix(new int[,] { { -1, 0, -1 }, { 0, 4, 0 }, { -1, 0, -1 } }, 1, 127));

    /// <inheritdoc/>
    public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
}
