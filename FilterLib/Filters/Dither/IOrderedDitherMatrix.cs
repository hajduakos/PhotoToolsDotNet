namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Interface for ordered dither matrices.
    /// </summary>
    public interface IOrderedDitherMatrix
    {
        /// <summary>
        /// Get an element of the matrix.
        /// </summary>
        /// <param name="x">Column number</param>
        /// <param name="y">Row number</param>
        /// <returns>Element</returns>
        float this[int x, int y] { get; }

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
