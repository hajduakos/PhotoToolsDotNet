namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Class representing a Bayer (ordered) dither matrix. The width and height of the matrix
    /// is a power of two. The base matrix is [ 0 2 ; 3 1 ] which can be recursively scaled up.
    /// </summary>
    public sealed class BayerDitherMatrix : IOrderedDitherMatrix
    {
        private readonly int size;
        private readonly float[,] matrix; // Matrix for caching

        /// <inheritdoc/>
        public float this[int x, int y] =>
                matrix != null ? matrix[x, y] : GetItem(x, y); // Use cached matrix or calculate on demand

        /// <inheritdoc/>
        public int Width { get; private set; }

        /// <inheritdoc/>
        public int Height { get; private set; }

        /// <summary>
        /// Constructor with size: width and height will be 2^size.
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
                for (int x = 0; x < Width; ++x)
                    for (int y = 0; y < Height; ++y)
                        matrix[x, y] = GetItem(x, y);
            }
        }

        private float GetItem(int x, int y) => (1f + GetItem(x, y, size)) / (1f + Width * Height);

        private int GetItem(int x, int y, int s)
        {
            if (s == 1)
                return baseMatrix[x * 2 + y];
            else
                return 4 * GetItem(x % (1 << (s - 1)), y % (1 << (s - 1)), s - 1) + baseMatrix[x / (1 << (s - 1)) * 2 + y / (1 << (s - 1))];
        }

        private static readonly int[] baseMatrix = new int[] { 0, 2, 3, 1 };
    }
}
