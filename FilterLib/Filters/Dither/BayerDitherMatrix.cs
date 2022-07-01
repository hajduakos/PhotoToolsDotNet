﻿namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Class representing a Bayer (ordered) dither matrix.
    /// </summary>
    public sealed class BayerDitherMatrix : IOrderedDitherMatrix
    {
        private readonly int size;
        private readonly float[,] matrix; // Matrix for caching

        /// <inheritdoc/>
        public float this[int i, int j] =>
                matrix != null ? matrix[i, j] : GetItem(i, j); // Use cached matrix or calculate on demand

        /// <inheritdoc/>
        public int Width { get; private set; }

        /// <inheritdoc/>
        public int Height { get; private set; }

        /// <summary>
        /// Constructor with size, width and height will be 2^size
        /// </summary>
        /// <param name="size"></param>
        public BayerDitherMatrix(int size)
        {
            this.size = size;
            Width = Height = 1 << size;
            matrix = null;
            // Use cached matrix for small size
            if (size <= 8)
            {
                matrix = new float[Width, Height];
                for (int i = 0; i < Width; ++i)
                    for (int j = 0; j < Height; ++j)
                        matrix[i, j] = GetItem(i, j);
            }
        }

        private float GetItem(int i, int j) => (1f + GetItem(i, j, size)) / (1f + Width * Height);

        private int GetItem(int i, int j, int s)
        {
            if (s == 1)
                return baseMatrix[i * 2 + j];
            else
                return 4 * GetItem(i % (1 << (s - 1)), j % (1 << (s - 1)), s - 1) + baseMatrix[i / (1 << (s - 1)) * 2 + j / (1 << (s - 1))];
        }

        private static readonly int[] baseMatrix = new int[] { 0, 2, 3, 1 };
    }
}
