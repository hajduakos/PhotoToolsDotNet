using FilterLib.Reporting;
using FilterLib.Util;
using System.Drawing;
using System.Drawing.Imaging;

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

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                int x, y;
                int w = image.Width;
                int wMul3 = w * 3;
                int h = image.Height;
                if (reporter != null) reporter.Report(0, 0, 2 * h - 1);
                float[,] turbulence = GenerateTurbulence(w, h);
                if (reporter != null) reporter.Report(h, 0, 2 * h - 1);


                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; y++)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                        {
                            row[x] = row[x + 1] = row[x + 2] = (byte)(turbulence[x / 3, y] * 255);
                        }
                        if ((y & 63) == 0) reporter?.Report(h + y, 0, 2 * h - 1);
                    }
                }

            }
            reporter?.Done();
        }
    }
}
