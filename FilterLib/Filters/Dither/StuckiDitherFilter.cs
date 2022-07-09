namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Stucki error diffusion dither filter.
    /// </summary>
    [Filter]
    public sealed class StuckiDitherFilter : ErrorDiffusionDitherFilterBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public StuckiDitherFilter(int levels = 256)
            // Error diffusion matrix
            // |    * 8 4|
            // |2 4 8 4 2|
            // |1 2 4 2 1| / 42
            : base(levels, new ErrorDiffusionMatrix(new float[,] {
            {0, 2, 1},
            {0, 4, 2},
            {0, 8, 4},
            {8, 4, 2},
            {4, 2, 1}}, 2))
        { }
    }
}
