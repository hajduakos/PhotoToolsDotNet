using FilterLib.Reporting;

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
            reporter?.Report(0, 0, 2 * image.Height - 1);
            float[,] turb = GenerateTurbulence(image.Width, image.Height);
            reporter?.Report(image.Height, 0, 2 * image.Height - 1);
            fixed (byte* start = image)
            {
                byte* ptr = start;
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        ptr[0] = ptr[1] = ptr[2] = (byte)(turb[x, y] * 255);
                        ptr += 3;
                    }
                    reporter?.Report(image.Height + y, 0, 2 * image.Height - 1);
                }
            }
            reporter?.Done();
        }
    }
}
