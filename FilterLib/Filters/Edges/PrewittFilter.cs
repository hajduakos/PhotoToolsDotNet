namespace FilterLib.Filters.Edges;

[Filter("Highlight edges by combining horizontal and vertical gradients (Prewitt operator).")]
public sealed class PrewittFilter : DualConvolutionBase
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public PrewittFilter() : base(
        new(new int[,] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } }, 1, 0),
        new(new int[,] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } }, 1, 0))
    { }
}
