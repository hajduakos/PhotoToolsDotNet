using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Base class for dither filters that work with an error diffusion matrix.
    /// </summary>
    public abstract class ErrorDiffusionDitherFilterBase : FilterInPlaceBase
    {
        private int levels;
        private readonly ErrorDiffusionMatrix matrix;

        /// <summary>
        /// Number of levels [2:256].
        /// </summary>
        [FilterParam]
        [FilterParamMin(2)]
        [FilterParamMax(256)]
        public int Levels
        {
            get { return levels; }
            set { levels = value.Clamp(2, 256); }
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
            int width_3 = image.Width * 3;
            float intervalSize = 255f / (levels - 1);
            float[,] quantErrArray = new float[width_3, image.Height];
            for (int x = 0; x < width_3; x++)
                for (int y = 0; y < image.Height; y++)
                    quantErrArray[x, y] = 0;

            float[,] diffusionMatrix = matrix.CopyMatrix();
            fixed (byte* start = image)
            {
                byte* ptr = start;
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < width_3; ++x)
                    {
                        // Add possible quantization error from before
                        byte pxWithErrorAdded = (*ptr + quantErrArray[x, y]).ClampToByte();
                        // Get rounded color
                        byte roundedColor = (System.MathF.Round(pxWithErrorAdded / intervalSize) * intervalSize).ClampToByte();
                        // Calculate current quantization error
                        int quantErr = pxWithErrorAdded - roundedColor;
                        // Distribute quantization error
                        // First row
                        for (int xSub = matrix.Offset + 1; xSub < matrix.Width; ++xSub)
                            if (x + (xSub - matrix.Offset) * 3 < width_3)
                                quantErrArray[x + (xSub - matrix.Offset) * 3, y] += quantErr * diffusionMatrix[xSub, 0];

                        // Other rows
                        for (int ySub = 1; ySub < matrix.Height; ++ySub)
                            if (y + ySub < image.Height)
                                for (int xSub = 0; xSub < matrix.Width; ++xSub)
                                    if (x + (xSub - matrix.Offset) * 3 < width_3 && x + (xSub - matrix.Offset) * 3 >= 0)
                                        quantErrArray[x + (xSub - matrix.Offset) * 3, y + ySub] += quantErr * diffusionMatrix[xSub, ySub];

                        // Replace color
                        *ptr = roundedColor;
                        ++ptr;
                    }
                    reporter?.Report(y + 1, 0, image.Height);
                }
            }
            reporter?.Done();
        }
    }
}
