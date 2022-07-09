namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Shiau-Fan error diffusion dither filter.
    /// </summary>
    [Filter]
    public sealed class ShiauFanDitherFilter : ErrorDiffusionDitherFilterBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public ShiauFanDitherFilter(int levels = 256)
            // Error diffusion matrix
            // |    * 4|
            // |1 1 2  | / 8
            : base(levels, new ErrorDiffusionMatrix(new float[,] {
            {0, 1},
            {0, 1},
            {0, 2},
            {4, 0}}, 2))
        { }
    }
}
