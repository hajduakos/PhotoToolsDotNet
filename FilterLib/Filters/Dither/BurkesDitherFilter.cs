namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Burkes dither filter.
    /// </summary>
    [Filter]
    public sealed class BurkesDitherFilter : ErrorDiffusionDitherFilterBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public BurkesDitherFilter(int levels = 256)
            // Error diffusion matrix
            // |    * 4 2|
            // |1 2 4 2 1| / 16
            : base(levels, new ErrorDiffusionMatrix(new float[,] {
            {0, 1},
            {0, 2},
            {0, 4},
            {4, 2},
            {2, 1}}, 2))
        { }
    }
}
