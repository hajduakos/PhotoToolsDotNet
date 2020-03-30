namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Sierra dither filter.
    /// </summary>
    [Filter]
    public sealed class SierraDitherFilter : ErrorDiffusionDitherFilterBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public SierraDitherFilter(int levels = 256)
            // Error diffusion matrix
            // |    * 5 3|
            // |2 4 5 4 2|
            // |  2 3 2  | / 32
            : base(levels, new ErrorDiffusionMatrix(new float[,] {
            {0, 2, 0},
            {0, 4, 2},
            {0, 5, 3},
            {5, 4, 2},
            {3, 2, 0}}, 2))
        { }
    }
}
