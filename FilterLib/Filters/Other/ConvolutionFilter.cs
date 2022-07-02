using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Other
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
        public ConvolutionFilter(Conv3x3 matrix = new Conv3x3()) => Matrix = matrix;

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            using (Bitmap original = (Bitmap)image.Clone())
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdOrig = new(original, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3;
                int h = image.Height;
                int x, y, nVal, stride = bmd.Stride;
                int[,] convMatrix = Matrix.CopyMatrix();

                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmd.Scan0 + (y * stride);
                        byte* rowOrg = (byte*)bmdOrig.Scan0 + (y * stride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; ++x)
                        {
                            byte mm = rowOrg[x];
                            byte tl = y > 0 && x >= 3 ? rowOrg[x - stride - 3] : mm;
                            byte tm = y > 0 ? rowOrg[x - stride] : mm;
                            byte tr = y > 0 && x < wMul3 - 4 ? rowOrg[x - stride + 3] : mm;
                            byte ml = x >= 3 ? rowOrg[x - 3] : mm;
                            byte mr = x < wMul3 - 4 ? rowOrg[x + 3] : mm;
                            byte bl = y < h - 1 && x >= 3 ? rowOrg[x + stride - 3] : mm;
                            byte bm = y < h - 1 ? rowOrg[x + stride] : mm;
                            byte br = y < h - 1 && x < wMul3 - 4 ? rowOrg[x + stride + 3] : mm;
                            nVal = (
                                tl * convMatrix[0, 0] + tm * convMatrix[1, 0] + tr * convMatrix[2, 0] +
                                ml * convMatrix[0, 1] + mm * convMatrix[1, 1] + mr * convMatrix[2, 1] +
                                bl * convMatrix[0, 2] + bm * convMatrix[1, 2] + br * convMatrix[2, 2])
                                / Matrix.Divisor + Matrix.Bias;

                            row[x] = (byte)nVal.Clamp(0, 255);
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
