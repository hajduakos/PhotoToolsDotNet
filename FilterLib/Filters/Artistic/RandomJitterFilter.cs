using FilterLib.Reporting;
using Math = System.Math;
using Random = System.Random;

namespace FilterLib.Filters.Artistic
{
    /// <summary>
    /// Random jitter filter.
    /// </summary>
    [Filter]
    public sealed class RandomJitterFilter : FilterInPlaceBase
    {
        private int radius;

        /// <summary>
        /// Jitter radius property.
        /// </summary>
        [FilterParam]
        [FilterParamMin(1)]
        public int Radius
        {
            get { return radius; }
            set { radius = Math.Max(1, value); }
        }

        /// <summary>
        /// Random number generator seed.
        /// </summary>
        [FilterParam]
        public int Seed { get; set; }

        /// <summary>
        /// Constructor with radius and seed parameters.
        /// </summary>
        /// <param name="radius">Jitter radius</param>
        /// <param name="seed">Random number generator seed</param>
        public RandomJitterFilter(int radius = 1, int seed = 0)
        {
            Radius = radius;
            Seed = seed;
        }

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
                    Random rnd = new(Seed);
                    int width_3 = image.Width * 3;
                    // Iterate through rows
                    for (int y = 0; y < image.Height; ++y)
                    {
                        // Get rows
                        byte* row = start + (y * width_3);
                        byte* rowOrig = origStart + (y * width_3);
                        // Iterate through columns
                        for (int x = 0; x < width_3; x += 3)
                        {
                            // Random numbers between -radius and +radius
                            int dx = rnd.Next(2 * radius + 1) - radius;
                            int dy = rnd.Next(2 * radius + 1) - radius;
                            dx *= 3; // Multiply by 3 because of the 3 components

                            // When out of range, take zero instead
                            if (x / 3 + dx < 0 || x / 3 + dx >= image.Width) dx = 0;
                            if (y + dy < 0 || y + dy >= image.Height) dy = 0;

                            // Calculate index (dy rows, dx columns)
                            int idx = dy * width_3 + dx;

                            // Replace all 3 components
                            row[x] = rowOrig[x + idx];
                            row[x + 1] = rowOrig[x + idx + 1];
                            row[x + 2] = rowOrig[x + idx + 2];
                        }
                        reporter?.Report(y, 0, image.Height - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
