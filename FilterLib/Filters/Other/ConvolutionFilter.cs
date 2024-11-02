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
        public ConvolutionMatrix Matrix { get; set; }

        /// <summary>
        /// Default constructor representing the identity function.
        /// </summary>
        public ConvolutionFilter() : this(new ConvolutionMatrix()) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="matrix">Convolution matrix</param>
        public ConvolutionFilter(ConvolutionMatrix matrix) => Matrix = matrix;

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
            int center_x = Matrix.Width / 2;
            int center_y = Matrix.Height / 2;
            fixed (byte* newStart = image, oldStart = original)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                Parallel.For(0, image.Height, y =>
                {
                    byte* newPtr = newStart0 + y * width_3;
                    for (int x = 0; x < width_3; ++x)
                    {
                        float sum = 0;
                        for (int mx = 0; mx < Matrix.Width; ++mx)
                        {
                            int oldX = (x / 3 - center_x + mx).Clamp(0, image.Width - 1) * 3 + x % 3;
                            for (int my = 0; my < Matrix.Height; ++my)
                            {
                                int oldY = (y - center_y + my).Clamp(0, image.Height - 1);
                                sum += oldStart0[oldY * width_3 + oldX] * Matrix[mx, my];
                            }
                        }
                        *newPtr = (sum / Matrix.Divisor + Matrix.Bias).ClampToByte();
                        ++newPtr;
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }
            reporter?.Done();
        }
    }
}