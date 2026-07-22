namespace FilterLib.Filters.Dither;

[Filter("Reduce color levels, spreading rounding error to neighbors (wider Shiau-Fan).")]
public sealed class ShiauFanDitherWideFilter : ErrorDiffusionDitherFilterBase
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="levels">Number of levels [2:256]</param>
    public ShiauFanDitherWideFilter(int levels = 256)
        // Error diffusion matrix
        // |      * 8|
        // |1 1 2 4  | / 16
        : base(levels, new ErrorDiffusionMatrix(new float[,] {
        {0, 1},
        {0, 1},
        {0, 2},
        {0, 4},
        {8, 0}}, 3))
    { }
}
