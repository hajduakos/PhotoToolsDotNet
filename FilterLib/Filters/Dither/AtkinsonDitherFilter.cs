namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Atkinson dither filter.
    /// </summary>
    [Filter]
    public sealed class AtkinsonDitherFilter : ErrorDiffusionDitherFilterBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public AtkinsonDitherFilter(int levels = 256)
            // Error diffusion matrix
            // |  * 1 1|
            // |1 1 1  |
            // |  1    | / 8
            : base(levels, new ErrorDiffusionMatrix(new float[,] {
            {0.0f, 1/8f, 0.0f},
            {0.0f, 1/8f, 1/8f},
            {1/8f, 1/8f, 0.0f},
            {1/8f, 0.0f, 0.0f}}, 1, false))
        { }
    }
}
