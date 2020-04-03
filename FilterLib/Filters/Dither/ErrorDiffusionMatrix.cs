using System;

namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Error diffusion matrix, e.g.,
    /// |   * 7 |
    /// | 3 5 1 |.
    /// </summary>
    public struct ErrorDiffusionMatrix
    {
        private readonly float[,] matrix; // Matrix

        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="i">Column number</param>
        /// <param name="j">Row number</param>
        /// <returns>Element of the matrix</returns>
        public float this[int i, int j] => matrix[i, j];

        /// <summary>
        /// Width of the matrix
        /// </summary>
        public readonly int Width;

        /// <summary>
        /// Height of the matrix
        /// </summary>
        public readonly int Height;

        /// <summary>
        /// Position of the actual pixel in the first row
        /// </summary>
        public readonly int Offset;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="matrix">Matrix</param>
        /// <param name="offset">Position of the actual pixel in the first row</param>
        /// <param name="normalize">Normalize the sum of matrix to 1</param>
        public ErrorDiffusionMatrix(float[,] matrix, int offset, bool normalize = true)
        {
            this.Width = matrix.GetLength(0);
            this.Height = matrix.GetLength(1);
            this.matrix = new float[Width, Height];
            this.Offset = offset;
            // Copy data and normalize
            float sum = 0;
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    this.matrix[x, y] = matrix[x, y];
                    if (normalize) sum += matrix[x, y];
                }
            }

            if (normalize)
                for (int x = 0; x < Width; ++x)
                    for (int y = 0; y < Height; ++y)
                        this.matrix[x, y] /= sum;
        }

        /// <summary>
        /// Create a copy of the matrix
        /// </summary>
        /// <returns>A new copy of the matrix</returns>
        public float[,] CopyMatrix()
        {
            float[,] copy = new float[Width, Height];
            Array.Copy(matrix, copy, Width * Height);
            return copy;
        }
    }
}
