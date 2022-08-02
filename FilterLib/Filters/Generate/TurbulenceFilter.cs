using FilterLib.Reporting;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Generate
{
    /// <summary>
    /// Generate turbulance.
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
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            reporter?.Report(0, 0, 2 * image.Height);
            float[,] turb = GenerateTurbulence(image.Width, image.Height);
            reporter?.Report(image.Height, 0, 2 * image.Height);
            fixed (byte* start = image)
            {
                byte* start0 = start;
                Parallel.For(0, image.Height, y =>
                {
                    byte* ptr = start0 + y * image.Width * 3;
                    for (int x = 0; x < image.Width; ++x)
                    {
                        ptr[0] = ptr[1] = ptr[2] = (byte)(turb[x, y] * 255);
                        ptr += 3;
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(image.Height + ++progress, 0, 2 * image.Height);
                });
            }
            reporter?.Done();
        }
    }
}
