namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Fan error diffusion dither filter.
    /// </summary>
    [Filter]
    public sealed class FanDitherFilter : ErrorDiffusionDitherFilterBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public FanDitherFilter(int levels = 256)
            // Error diffusion matrix
            // |    * 7|
            // |1 3 5 0| / 16
            : base(levels, new ErrorDiffusionMatrix(new float[,] {
            {0, 1},
            {0, 3},
            {0, 5},
            {7, 0}}, 2))
        { }
    }
}
