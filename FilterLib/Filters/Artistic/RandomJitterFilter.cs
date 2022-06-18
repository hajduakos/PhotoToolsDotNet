using System;
using System.Drawing;
using System.Drawing.Imaging;
using FilterLib.Reporting;
using FilterLib.Util;

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
            set { radius = System.Math.Max(1, value); }
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
            this.Radius = radius;
            this.Seed = seed;
        }

        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            using (Bitmap original = (Bitmap)image.Clone())
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdOrg = new(original, PixelFormat.Format24bppRgb))
            {
                // Random number generator
                Random rnd = new(Seed);
                int dx, dy, idx, w = image.Width, h = image.Height;
                int stride = bmd.Stride;
                int wMul3 = image.Width * 3; // Width of a row
                int x, y;
                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmd.Scan0 + (y * stride);
                        byte* rowOrg = (byte*)bmdOrg.Scan0 + (y * stride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                        {
                            // Random numbers between -radius and +radius
                            dx = rnd.Next(2 * radius + 1) - radius;
                            dy = rnd.Next(2 * radius + 1) - radius;
                            dx *= 3; // Multiply by 3 because of the 3 components

                            // When out of range, take zero instead
                            if (x / 3 + dx < 0 || x / 3 + dx >= w) dx = 0;
                            if (y + dy < 0 || y + dy >= h) dy = 0;

                            // Calculate index (dy rows, dx columns)
                            idx = dy * stride + dx;

                            // Replace all 3 components
                            row[x] = rowOrg[x + idx];
                            row[x + 1] = rowOrg[x + idx + 1];
                            row[x + 2] = rowOrg[x + idx + 2];
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
