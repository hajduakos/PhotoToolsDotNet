namespace FilterLib.Filters.Dither
{
    [Filter("Jarvis-Judice-Ninke error diffusion dither.")]
    public sealed class JarvisJudiceNinkeDitherFilter : ErrorDiffusionDitherFilterBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public JarvisJudiceNinkeDitherFilter(int levels = 256)
            // Error diffusion matrix
            // |    * 7 5|
            // |3 5 7 5 3|
            // |1 3 5 3 1| / 48
            : base(levels, new ErrorDiffusionMatrix(new float[,] {
            {0, 3, 1},
            {0, 5, 3},
            {0, 7, 5},
            {7, 5, 3},
            {5, 3, 1}}, 2))
        { }
    }
}
