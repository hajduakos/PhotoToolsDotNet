namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Sierra error diffusion dither filter with two rows only.
    /// </summary>
    [Filter]
    public sealed class SierraDitherTwoRowFilter : ErrorDiffusionDitherFilterBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public SierraDitherTwoRowFilter(int levels = 256)
            // Error diffusion matrix
            // |    * 4 3|
            // |1 2 3 2 1| / 16
            : base(levels, new ErrorDiffusionMatrix(new float[,] {
            {0, 1},
            {0, 2},
            {0, 3},
            {4, 2},
            {3, 1}}, 2))
        { }
    }
}
