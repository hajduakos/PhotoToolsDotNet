namespace FilterLib.Filters.Edges;

[Filter("Highlight edges by combining horizontal and vertical gradients (Scharr operator).")]
public sealed class ScharrFilter : DualConvolutionBase
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public ScharrFilter() : base(
        new(new int[,] { { 3, 10, 3 }, { 0, 0, 0 }, { -3, -10, -3 } }),
        new(new int[,] { { 3, 0, -3 }, { 10, 0, -10 }, { 3, 0, -3 } }))
    { }
}
