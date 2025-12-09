using FilterLib.Reporting;
using FilterLib.Util;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Base class for dither filters that work with an error diffusion matrix.
    /// </summary>
    public abstract class ErrorDiffusionDitherFilterBase : FilterInPlaceBase
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

        private readonly ErrorDiffusionMatrix matrix;

        /// <summary>
        /// Number of levels [2:256].
        /// </summary>
        [FilterParam]
        [FilterParamMin(2)]
        [FilterParamMax(256)]
        public int Levels
        {
            get;
            set { field = value.Clamp(2, 256); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2;256]</param>
        /// <param name="matrix">Error diffusion matrix</param>
        protected ErrorDiffusionDitherFilterBase(int levels, ErrorDiffusionMatrix matrix)
        {
            Levels = levels;
            this.matrix = matrix;
        }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            int width_3 = image.Width * 3;
            float intervalSize = 255f / (Levels - 1);
            float[,] quantErrArray = new float[width_3, image.Height];
            for (int x = 0; x < width_3; x++)
                for (int y = 0; y < image.Height; y++)
                    quantErrArray[x, y] = 0;

            fixed (byte* start = image)
            {
                byte* start0 = start;
                // Components can be processed independently in parallel
                Parallel.For(0, 3, comp =>
                {
                    byte* ptr = start0 + comp;
                    for (int y = 0; y < image.Height; y++)
                    {
                        for (int x = 0; x < width_3; x += 3)
                        {
                            // Add possible quantization error from before
                            byte pxWithErrorAdded = (*ptr + quantErrArray[x + comp, y]).ClampToByte();
                            // Get rounded color
                            byte roundedColor = (System.MathF.Round(pxWithErrorAdded / intervalSize) * intervalSize).ClampToByte();
                            // Calculate current quantization error
                            int quantErr = pxWithErrorAdded - roundedColor;
                            // Distribute quantization error
                            // First row
                            for (int xSub = matrix.Offset + 1; xSub < matrix.Width; ++xSub)
                                if (x + comp + (xSub - matrix.Offset) * 3 < width_3)
                                    quantErrArray[x + comp + (xSub - matrix.Offset) * 3, y] += quantErr * matrix[xSub, 0];

                            // Other rows
                            for (int ySub = 1; ySub < matrix.Height; ++ySub)
                                if (y + ySub < image.Height)
                                    for (int xSub = 0; xSub < matrix.Width; ++xSub)
                                        if (x + comp + (xSub - matrix.Offset) * 3 < width_3 && x + comp + (xSub - matrix.Offset) * 3 >= 0)
                                            quantErrArray[x + comp + (xSub - matrix.Offset) * 3, y + ySub] += quantErr * matrix[xSub, ySub];

                            // Replace color
                            *ptr = roundedColor;
                            ptr += 3;
                        }
                        if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height * 3);
                    }
                });
            }
            reporter?.Done();
        }
    }
}
