using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

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
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                int pxWithErrorAdded;
                int width_3 = image.Width * 3;
                int h = image.Height;
                int x, y;
                int xSub, ySub;
                float intervalSize = 255f / (levels - 1); // Size of an interval
                int roundedColor; // Color rounded to the nearest color level
                float quantErr;
                float[,] quantErrArray = new float[width_3, h];
                for (x = 0; x < width_3; x++) for (y = 0; y < h; y++) quantErrArray[x, y] = 0;

                float[,] diffusionMatrix = matrix.CopyMatrix();

                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; y++)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        // Iterate through columns
                        for (x = 0; x < width_3; ++x)
                        {
                            // Add quantization error
                            pxWithErrorAdded = (int)(row[x] + quantErrArray[x, y]);
                            if (pxWithErrorAdded > 255) pxWithErrorAdded = 255;
                            else if (pxWithErrorAdded < 0) pxWithErrorAdded = 0;
                            // Get rounded color
                            roundedColor = (int)(System.MathF.Round(pxWithErrorAdded / intervalSize) * intervalSize);
                            // Calculate quantization error
                            quantErr = pxWithErrorAdded - roundedColor;
                            // Sum quantization error
                            // First row
                            for (xSub = matrix.Offset + 1; xSub < matrix.Width; ++xSub)
                                if (x + (xSub - matrix.Offset) * 3 < width_3)
                                    quantErrArray[x + (xSub - matrix.Offset) * 3, y] += quantErr * diffusionMatrix[xSub, 0];

                            // Other rows
                            for (ySub = 1; ySub < matrix.Height; ++ySub)
                                if (y + ySub < h)
                                    for (xSub = 0; xSub < matrix.Width; ++xSub)
                                        if (x + (xSub - matrix.Offset) * 3 < width_3 && x + (xSub - matrix.Offset) * 3 >= 0)
                                            quantErrArray[x + (xSub - matrix.Offset) * 3, y + ySub] += quantErr * diffusionMatrix[xSub, ySub];

                            // Replace color
                            row[x] = (byte)roundedColor;
                        }
                        reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
