namespace FilterLib.Filters.Edges;

[Filter("Highlight edges by combining horizontal and vertical gradients (Sobel operator).")]
public sealed class SobelFilter : DualConvolutionBase
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public SobelFilter() : base(
        new(new int[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } }),
        new(new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } }))
    { }
}
