using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Noise
{
    /// <summary>
    /// Median filter.
    /// </summary>
    [Filter]
    public sealed class MedianFilter : FilterInPlaceBase
    {
        private int strength;

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
        /// Constructor.
        /// </summary>
        /// <param name="strength">Strength [0;100]</param>
        public MedianFilter(int strength = 0) => Strength = strength;

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(image.Width == original.Width);
            int width_3 = image.Width * 3;
            byte[,] neighRGBs = new byte[3, 9]; // Arrays for sorting
            byte[] neighLums = new byte[9];

            float op1 = strength / 100.0f;
            float op0 = 1 - op1;

            fixed (byte* newStart = image, oldStart = original)
            {
                for (int y = 1; y < image.Height - 1; ++y)
                {
                    byte* newRow = newStart + y * width_3;
                    byte* oldRow = oldStart + y * width_3;
                    for (int x = 3; x < width_3 - 3; x += 3)
                    {
                        // Collect pixel and surroundings
                        for (int k = 0; k < 3; ++k)
                        {
                            neighRGBs[k, 0] = oldRow[k + x - width_3 - 3];
                            neighRGBs[k, 1] = oldRow[k + x - width_3];
                            neighRGBs[k, 2] = oldRow[k + x - width_3 + 3];
                            neighRGBs[k, 3] = oldRow[k + x - 3];
                            neighRGBs[k, 4] = oldRow[k + x];
                            neighRGBs[k, 5] = oldRow[k + x + 3];
                            neighRGBs[k, 6] = oldRow[k + x + width_3 - 3];
                            neighRGBs[k, 7] = oldRow[k + x + width_3];
                            neighRGBs[k, 8] = oldRow[k + x + width_3 + 3];
                        }
                        // Calculate luminance values
                        for (int k = 0; k < 9; ++k)
                            neighLums[k] = (byte)RGB.GetLuminance(neighRGBs[0, k], neighRGBs[1, k], neighRGBs[2, k]);
                        // Sort by luminance (only the first 5 elements, since we need the 5th
                        for (int k = 0; k < 5; ++k)
                        {
                            int min = k;
                            for (int l = k + 1; l < 9; ++l)
                                if (neighLums[l] < neighLums[min]) min = l;
                            // Swap
                            if (k != min)
                            {
                                // Swap luminance
                                (neighLums[min], neighLums[k]) = (neighLums[k], neighLums[min]);
                                // Swap rgbs
                                for (int i = 0; i < 3; ++i)
                                {
                                    (neighRGBs[i, min], neighRGBs[i, k]) = (neighRGBs[i, k], neighRGBs[i, min]);
                                }
                            }
                        }
                        // Get the median
                        newRow[x] = (byte)(op0 * newRow[x] + op1 * neighRGBs[0, 4]);
                        newRow[x + 1] = (byte)(op0 * newRow[x + 1] + op1 * neighRGBs[1, 4]);
                        newRow[x + 2] = (byte)(op0 * newRow[x + 2] + op1 * neighRGBs[2, 4]);
                    }
                    reporter?.Report(y, 1, image.Height - 2);
                }
            }
            reporter?.Done();
        }
    }
}
