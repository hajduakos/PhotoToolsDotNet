using FilterLib.Reporting;
using FilterLib.Util;
using Math = System.Math;

namespace FilterLib.Filters.Noise
{
    /// <summary>
    /// Median filter.
    /// </summary>
    [Filter]
    public sealed class MedianFilter : FilterInPlaceBase
    {
        private int strength;
        private int radius;

        /// <summary>
        /// Strength [0;100].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(100)]
        public int Strength
        {
            get { return strength; }
            set { strength = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Radius [0;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int Radius
        {
            get { return radius; }
            set { radius = Math.Max(0, value); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="strength">Strength [0;100]</param>
        /// <param name="radius">Radius [0;...]</param>
        public MedianFilter(int strength = 0, int radius = 0)
        {
            Strength = strength;
            Radius = radius;
        }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(image.Width == original.Width);
            int width_3 = image.Width * 3;
            int radius_3 = radius * 3;
            int area = (2 * radius + 1) * (2 * radius + 1);
            (byte, byte, byte)[] neighRGBs = new (byte, byte, byte)[area]; // Arrays for sorting
            byte[] neighLums = new byte[area];

            float op1 = strength / 100.0f;
            float op0 = 1 - op1;

            fixed (byte* newStart = image, oldStart = original)
            {
                byte* newPx = newStart;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < width_3; x += 3)
                    {
                        // Collect pixel and surroundings
                        int n = 0;
                        for (int y0 = Math.Max(0, y - radius); y0 < image.Height && y0 <= y + radius; ++y0)
                        {
                            for (int x0 = Math.Max(0, x - radius_3); x0 < width_3 && x0 <= x + radius_3; x0 += 3)
                            {
                                byte* px = oldStart + y0 * width_3 + x0;
                                neighRGBs[n] = (px[0], px[1], px[2]);
                                neighLums[n] = (byte)RGB.GetLuminance(px[0], px[1], px[2]);
                                ++n;
                            }
                        }
                        int med = (int)Math.Ceiling(n / 2f);
                        // Sort by luminance (only up to the median)
                        for (int k = 0; k < med; ++k)
                        {
                            int min = k;
                            for (int l = k + 1; l < n; ++l)
                                if (neighLums[l] < neighLums[min]) min = l;
                            // Swap
                            if (k != min)
                            {
                                // Swap luminance
                                (neighLums[min], neighLums[k]) = (neighLums[k], neighLums[min]);
                                // Swap rgbs
                                (neighRGBs[min], neighRGBs[k]) = (neighRGBs[k], neighRGBs[min]);
                            }
                        }
                        // Get the median
                        (byte r, byte g, byte b) = neighRGBs[med - 1];
                        newPx[0] = (byte)(op0 * newPx[0] + op1 * r);
                        newPx[1] = (byte)(op0 * newPx[1] + op1 * g);
                        newPx[2] = (byte)(op0 * newPx[2] + op1 * b);
                        newPx += 3;
                    }
                    reporter?.Report(y + 1, 0, image.Height);
                }
            }
            reporter?.Done();
        }
    }
}
