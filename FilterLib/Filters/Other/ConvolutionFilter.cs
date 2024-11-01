using FilterLib.Reporting;
using FilterLib.Util;
using Parallel = System.Threading.Tasks.Parallel;

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
            object reporterLock = new();
            int progress = 0;
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(original.Width == image.Width);
            int width_3 = image.Width * 3;
            fixed (byte* newStart = image, oldStart = original)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                Parallel.For(0, image.Height, y =>
                {
                    byte* oldPtr = oldStart0 + y * width_3;
                    byte* newPtr = newStart0 + y * width_3;
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
                            tl * Matrix[0, 0] + tm * Matrix[1, 0] + tr * Matrix[2, 0] +
                            ml * Matrix[0, 1] + mm * Matrix[1, 1] + mr * Matrix[2, 1] +
                            bl * Matrix[0, 2] + bm * Matrix[1, 2] + br * Matrix[2, 2])
                            / Matrix.Divisor + Matrix.Bias;

                        *newPtr = nVal.ClampToByte();
                        ++newPtr;
                        ++oldPtr;
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }
            reporter?.Done();
        }
    }
}