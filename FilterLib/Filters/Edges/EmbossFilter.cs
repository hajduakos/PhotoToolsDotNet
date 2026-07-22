using ConvolutionFilter = FilterLib.Filters.Other.ConvolutionFilter;
using ConvolutionMatrix = FilterLib.Util.ConvolutionMatrix;
using IReporter = FilterLib.Reporting.IReporter;

namespace FilterLib.Filters.Edges;

[Filter("Turn the image into a gray relief where edges look raised or engraved.")]
public sealed class EmbossFilter : FilterInPlaceBase
{
    private readonly ConvolutionFilter conv = new(new ConvolutionMatrix(new int[,] { { 1, 0, -1 }, { 1, 0, -1 }, { 1, 0, -1 } }, 1, 127));

    /// <inheritdoc/>
    public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
}
