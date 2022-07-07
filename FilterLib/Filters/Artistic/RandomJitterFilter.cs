using FilterLib.Reporting;

namespace FilterLib.Filters.Artistic
{
    /// <summary>
    /// Create a jitter effect by replacing each pixel
    /// by a random one within a given radius.
    /// </summary>
    [Filter]
    public sealed class RandomJitterFilter : FilterInPlaceBase
    {
        private int radius;

        /// <summary>
        /// Jitter radius [0;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int Radius
        {
            get { return radius; }
            set { radius = System.Math.Max(0, value); }
        }

        /// <summary>
        /// Random number generator seed.
        /// </summary>
        [FilterParam]
        public int Seed { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="radius">Jitter radius [0;...]</param>
        /// <param name="seed">Random number generator seed</param>
        public RandomJitterFilter(int radius = 0, int seed = 0)
        {
            Radius = radius;
            Seed = seed;
        }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            System.Random rnd = new(Seed);
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(image.Width == original.Width);
            int width_3 = image.Width * 3;

            fixed (byte* newStart = image, oldStart = original)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    byte* newRow = newStart + y * width_3;
                    byte* oldRow = oldStart + y * width_3;
                    for (int x = 0; x < width_3; x += 3)
                    {
                        // Random numbers between -radius and +radius
                        int dx = rnd.Next(2 * radius + 1) - radius;
                        int dy = rnd.Next(2 * radius + 1) - radius;

                        // When out of range, take zero instead
                        if (x / 3 + dx < 0 || x / 3 + dx >= image.Width) dx = 0;
                        if (y + dy < 0 || y + dy >= image.Height) dy = 0;

                        // Calculate offset (dy rows, dx columns)
                        int idx = dy * width_3 + dx * 3;

                        newRow[x] = oldRow[x + idx];
                        newRow[x + 1] = oldRow[x + idx + 1];
                        newRow[x + 2] = oldRow[x + idx + 2];
                    }
                    reporter?.Report(y, 0, image.Height - 1);
                }
            }
            reporter?.Done();
        }
    }
}

