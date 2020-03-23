using FilterLib.Util;

namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Floyd-Steinberg dither filter.
    /// </summary>
    [Filter]
    public sealed class FloydSteinbergDitherFilter : ErrorDiffusionDitherFilterBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public FloydSteinbergDitherFilter(int levels = 256)
            // Error diffusion matrix
            // |  * 7|
            // |3 5 1| / 16
            : base(levels, new ErrorDiffusionMatrix(new float[,] {
            { 0, 3 },
            { 0, 5 },
            { 7, 1 } }, 1))
        { }
    }
}
