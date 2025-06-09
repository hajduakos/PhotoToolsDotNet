namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Filter lite error diffusion dither filter.
    /// </summary>
    [Filter]
    public sealed class FilterLiteDitherFilter : ErrorDiffusionDitherFilterBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public FilterLiteDitherFilter(int levels = 256)
            // Error diffusion matrix
            // |  * 2|
            // |1 1 0| / 4
            : base(levels, new ErrorDiffusionMatrix(new float[,] {
            {0, 1},
            {0, 1},
            {2, 0}}, 1, true))
        { }
    }
}
