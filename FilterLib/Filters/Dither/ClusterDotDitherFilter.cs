namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Cluster dot dither is an ordered dither with a specifically defined matrix
    /// </summary>
    [Filter]
    public sealed class ClusterDotDitherFilter : OrderedDitherFilterBase
    {
        /// <summary>
        /// Cluster dot dither matrix
        /// </summary>
        public sealed class ClusterDotDitherMatrix : IOrderedDitherMatrix
        {
            private readonly float[,] matrix;

            /// <summary>
            /// Constructor
            /// </summary>
            public ClusterDotDitherMatrix()
            {
                matrix = new float[8, 8]
                {
                { 24, 8, 22, 30, 34, 44, 42, 32 },
                { 10, 0, 6, 20, 46, 58, 56, 40 },
                { 12, 2, 4, 18, 48, 60, 62, 54 },
                { 26, 14, 16, 28, 36, 50, 52, 38 },
                { 35, 45, 43, 33, 25, 9, 23, 31 },
                { 47, 59, 57, 41, 11, 1, 7, 21 },
                { 49, 61, 63, 55, 13, 3, 5, 19 },
                { 37, 51, 53, 39, 27, 15, 17, 29 },
                };
                for (int x = 0; x < matrix.GetLength(0); x++)
                    for (int y = 0; y < matrix.GetLength(1); y++)
                        matrix[x, y] = (1f + matrix[x, y]) / 65f;
            }

            /// <inheritdoc/>
            public float this[int x, int y] => matrix[x, y];

            /// <inheritdoc/>
            public int Width { get { return matrix.GetLength(0); } }

            /// <inheritdoc/>
            public int Height { get { return matrix.GetLength(1); } }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public ClusterDotDitherFilter(int levels = 256) : base(levels, new ClusterDotDitherMatrix()) { }
    }
}
