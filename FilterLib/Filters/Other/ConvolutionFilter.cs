using FilterLib.Reporting;
using FilterLib.Util;

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
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();

            unsafe
            {
                fixed (byte* start = image, origStart = original)
                {
                    int width_3 = image.Width * 3;
                    int h = image.Height;
                    int x, y, nVal, stride = width_3;
                    int[,] convMatrix = Matrix.CopyMatrix();

                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get rows
                        byte* row = start + (y * stride);
                        byte* rowOrig = origStart + (y * stride);
                        // Iterate through columns
                        for (x = 0; x < width_3; ++x)
                        {
                            byte mm = rowOrig[x];
                            byte tl = y > 0 && x >= 3 ? rowOrig[x - stride - 3] : mm;
                            byte tm = y > 0 ? rowOrig[x - stride] : mm;
                            byte tr = y > 0 && x < width_3 - 4 ? rowOrig[x - stride + 3] : mm;
                            byte ml = x >= 3 ? rowOrig[x - 3] : mm;
                            byte mr = x < width_3 - 4 ? rowOrig[x + 3] : mm;
                            byte bl = y < h - 1 && x >= 3 ? rowOrig[x + stride - 3] : mm;
                            byte bm = y < h - 1 ? rowOrig[x + stride] : mm;
                            byte br = y < h - 1 && x < width_3 - 4 ? rowOrig[x + stride + 3] : mm;
                            nVal = (
                                tl * convMatrix[0, 0] + tm * convMatrix[1, 0] + tr * convMatrix[2, 0] +
                                ml * convMatrix[0, 1] + mm * convMatrix[1, 1] + mr * convMatrix[2, 1] +
                                bl * convMatrix[0, 2] + bm * convMatrix[1, 2] + br * convMatrix[2, 2])
                                / Matrix.Divisor + Matrix.Bias;

                            row[x] = nVal.ClampToByte();
                        }
                        reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
