namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Error diffusion matrix where an element in the first row is the actual pixel
    /// and the rest of the elements represent the distribution quantization error.
    /// 
    /// Example:
    /// 
    /// |   * 7 |
    /// | 3 5 1 |.
    /// </summary>
    public readonly struct ErrorDiffusionMatrix
    {
        private readonly float[,] matrix; // Matrix

        /// <summary>
        /// Indexer.
        /// </summary>
        /// <param name="x">Column number</param>
        /// <param name="y">Row number</param>
        /// <returns>Element of the matrix</returns>
        public readonly float this[int x, int y] => matrix[x, y];

        /// <summary>
        /// Width of the matrix.
        /// </summary>
        public readonly int Width;

        /// <summary>
        /// Height of the matrix.
        /// </summary>
        public readonly int Height;

        /// <summary>
        /// Position of the actual pixel in the first row.
        /// </summary>
        public readonly int Offset;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="matrix">Matrix</param>
        /// <param name="offset">Position of the actual pixel in the first row</param>
        /// <param name="normalize">Normalize the sum of matrix to 1</param>
        public ErrorDiffusionMatrix(float[,] matrix, int offset, bool normalize = true)
        {
            Width = matrix.GetLength(0);
            Height = matrix.GetLength(1);
            this.matrix = new float[Width, Height];
            Offset = offset;
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
    }
}
