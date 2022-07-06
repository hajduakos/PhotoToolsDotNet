using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Generate
{
    /// <summary>
    /// Turbulence generating filter.
    /// </summary>
    [Filter]
    public sealed class TurbulenceFilter : GeneratorBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="iterations">Number of iterations [1;...]</param>
        /// <param name="seed">Random number generator seed</param>
        public TurbulenceFilter(int iterations = 1, int seed = 0) : base(iterations, seed) { }

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            unsafe
            {
                fixed (byte* start = image)
                {
                    int x, y;
                    int w = image.Width;
                    int width_3 = w * 3;
                    int h = image.Height;
                    if (reporter != null) reporter.Report(0, 0, 2 * h - 1);
                    float[,] turbulence = GenerateTurbulence(w, h);
                    if (reporter != null) reporter.Report(h, 0, 2 * h - 1);


                    // Iterate through rows
                    for (y = 0; y < h; y++)
                    {
                        // Get row
                        byte* row = start + y * width_3;
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            row[x] = row[x + 1] = row[x + 2] = (byte)(turbulence[x / 3, y] * 255);
                        }
                        reporter?.Report(h + y, 0, 2 * h - 1);
                    }
                }

            }
            reporter?.Done();
        }
    }
}
