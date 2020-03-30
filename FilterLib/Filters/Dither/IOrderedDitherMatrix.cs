namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Ordered dither matrix interface
    /// </summary>
    public interface IOrderedDitherMatrix
    {
        /// <summary>
        /// Get an element of the matrix
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        float this[int i, int j] { get; }

        /// <summary>
        /// Width of the matrix
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of the matrix
        /// </summary>
        int Height { get; }
    }
}
