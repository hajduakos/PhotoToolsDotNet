using FilterLib.Filters;
using FilterLib.Reporting;
using FilterLib.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterLib.Other
{
    /// <summary>
    /// Convolution filter.
    /// </summary>
    [Filter]
    public sealed class ConvolutionFilter : FilterInPlaceBase
    {
        /// <summary>
        /// Convolution matrix.
        /// </summary>
        [FilterParam]
        public Conv3x3 Matrix { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="matrix">Convolution matrix</param>
        public ConvolutionFilter(Conv3x3 matrix = new Conv3x3())
        {
            this.Matrix = matrix;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            using (Bitmap original = (Bitmap)image.Clone())
            using (DisposableBitmapData bmd = new DisposableBitmapData(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdOrg = new DisposableBitmapData(original, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3;
                int h = image.Height;
                int x, y, r, g, b, stride = bmd.Stride;
                int[,] convMatrix = Matrix.CopyMatrix();


                unsafe
                {
                    // Iterate through rows
                    for (y = 1; y < h - 1; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmd.Scan0 + (y * stride);
                        byte* rowOrg = (byte*)bmdOrg.Scan0 + (y * stride);
                        // Iterate through columns
                        for (x = 3; x < wMul3 - 3; x += 3)
                        {
                            // Apply convolution on blue channel
                            b = (rowOrg[x - stride - 3] * convMatrix[0, 0] +
                                rowOrg[x - stride] * convMatrix[1, 0] +
                                rowOrg[x - stride + 3] * convMatrix[2, 0] +
                                rowOrg[x - 3] * convMatrix[0, 1] +
                                rowOrg[x] * convMatrix[1, 1] +
                                rowOrg[x + 3] * convMatrix[2, 1] +
                                rowOrg[x + stride - 3] * convMatrix[0, 2] +
                                rowOrg[x + stride] * convMatrix[1, 2] +
                                rowOrg[x + stride + 3] * convMatrix[2, 2]) / Matrix.Divisor + Matrix.Bias;

                            // Apply convolution on green channel
                            g = (rowOrg[x + 1 - stride - 3] * convMatrix[0, 0] +
                                rowOrg[x + 1 - stride] * convMatrix[1, 0] +
                                rowOrg[x + 1 - stride + 3] * convMatrix[2, 0] +
                                rowOrg[x + 1 - 3] * convMatrix[0, 1] +
                                rowOrg[x + 1] * convMatrix[1, 1] +
                                rowOrg[x + 1 + 3] * convMatrix[2, 1] +
                                rowOrg[x + 1 + stride - 3] * convMatrix[0, 2] +
                                rowOrg[x + 1 + stride] * convMatrix[1, 2] +
                                rowOrg[x + 1 + stride + 3] * convMatrix[2, 2]) / Matrix.Divisor + Matrix.Bias;

                            // Apply convolution on red channel
                            r = (rowOrg[x + 2 - stride - 3] * convMatrix[0, 0] +
                                rowOrg[x + 2 - stride] * convMatrix[1, 0] +
                                rowOrg[x + 2 - stride + 3] * convMatrix[2, 0] +
                                rowOrg[x + 2 - 3] * convMatrix[0, 1] +
                                rowOrg[x + 2] * convMatrix[1, 1] +
                                rowOrg[x + 2 + 3] * convMatrix[2, 1] +
                                rowOrg[x + 2 + stride - 3] * convMatrix[0, 2] +
                                rowOrg[x + 2 + stride] * convMatrix[1, 2] +
                                rowOrg[x + 2 + stride + 3] * convMatrix[2, 2]) / Matrix.Divisor + Matrix.Bias;

                            // New values
                            row[x] = (byte)b.Clamp(0, 255);
                            row[x + 1] = (byte)g.Clamp(0, 255);
                            row[x + 2] = (byte)r.Clamp(0, 255);
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
