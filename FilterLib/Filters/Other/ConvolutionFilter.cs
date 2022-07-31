using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Other
{
    /// <summary>
    /// Convolution filter using a given 3x3 kernel (matrix).
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
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(original.Width == image.Width);
            int width_3 = image.Width * 3;
            int[,] mx = Matrix.CopyMatrix();
            fixed (byte* newStart = image, oldStart = original)
            {
                byte* newPtr = newStart;
                byte* oldPtr = oldStart;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < width_3; ++x)
                    {
                        byte mm = *oldPtr;
                        byte tl = y > 0 && x >= 3 ? oldPtr[-width_3 - 3] : mm;
                        byte tm = y > 0 ? oldPtr[-width_3] : mm;
                        byte tr = y > 0 && x < width_3 - 3 ? oldPtr[-width_3 + 3] : mm;
                        byte ml = x >= 3 ? oldPtr[-3] : mm;
                        byte mr = x < width_3 - 3 ? oldPtr[3] : mm;
                        byte bl = y < image.Height - 1 && x >= 3 ? oldPtr[width_3 - 3] : mm;
                        byte bm = y < image.Height - 1 ? oldPtr[width_3] : mm;
                        byte br = y < image.Height - 1 && x < width_3 - 3 ? oldPtr[width_3 + 3] : mm;
                        float nVal = (
                            tl * mx[0, 0] + tm * mx[1, 0] + tr * mx[2, 0] +
                            ml * mx[0, 1] + mm * mx[1, 1] + mr * mx[2, 1] +
                            bl * mx[0, 2] + bm * mx[1, 2] + br * mx[2, 2])
                            / Matrix.Divisor + Matrix.Bias;

                        *newPtr = nVal.ClampToByte();
                        ++newPtr;
                        ++oldPtr;
                    }
                    reporter?.Report(y + 1, 0, image.Height);
                }
            }
            reporter?.Done();
        }
    }
}