using ConvolutionFilter = FilterLib.Filters.Other.ConvolutionFilter;
using ConvolutionMatrix = FilterLib.Util.ConvolutionMatrix;
using IReporter = FilterLib.Reporting.IReporter;

namespace FilterLib.Filters.Blur;

[Filter("Apply a light, fixed-strength blur to gently soften the image.")]
public sealed class BlurFilter : FilterInPlaceBase
{
    private readonly ConvolutionFilter conv = new(new ConvolutionMatrix(new int[,] { { 1, 1, 1 }, { 1, 8, 1 }, { 1, 1, 1 } }, 16, 0));

    /// <inheritdoc/>
    public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
}
