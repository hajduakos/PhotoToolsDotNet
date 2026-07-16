namespace FilterLib.Filters.Edges;

[Filter("Sobel filter combining two convolutions.")]
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
