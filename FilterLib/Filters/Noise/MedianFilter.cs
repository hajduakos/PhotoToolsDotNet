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
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();

            unsafe
            {
                fixed (byte* start = image, origStart = original)
                {
                    int width_3 = image.Width * 3;
                    int h = image.Height;
                    int x, y, k, l, min, stride = width_3;
                    byte[,] medianBGR = new byte[3, 9]; // Arrays for sorting
                    byte[] medianLum = new byte[9];
                    byte swap;

                    float op1 = strength / 100.0f;
                    float op0 = 1 - op1;

                    // Iterate through rows
                    for (y = 1; y < h - 1; ++y)
                    {
                        // Get rows
                        byte* row = start + (y * stride);
                        byte* rowOrig = origStart + (y * stride);
                        // Iterate through columns
                        for (x = 3; x < width_3 - 3; x += 3)
                        {
                            // Collect pixel and surroundings
                            for (k = 0; k < 3; ++k)
                            {
                                medianBGR[k, 0] = rowOrig[k + x - stride - 3];
                                medianBGR[k, 1] = rowOrig[k + x - stride];
                                medianBGR[k, 2] = rowOrig[k + x - stride + 3];
                                medianBGR[k, 3] = rowOrig[k + x - 3];
                                medianBGR[k, 4] = rowOrig[k + x];
                                medianBGR[k, 5] = rowOrig[k + x + 3];
                                medianBGR[k, 6] = rowOrig[k + x + stride - 3];
                                medianBGR[k, 7] = rowOrig[k + x + stride];
                                medianBGR[k, 8] = rowOrig[k + x + stride + 3];
                            }
                            // Calculate luminance values
                            for (k = 0; k < 9; ++k)
                                medianLum[k] = (byte)RGB.GetLuminance(medianBGR[0, k], medianBGR[1, k], medianBGR[2, k]);
                            // Sort by luminance (only the first 5 elements, since we need the 5th
                            for (k = 0; k < 5; ++k)
                            {
                                min = k;
                                for (l = k + 1; l < 9; ++l)
                                    if (medianLum[l] < medianLum[min]) min = l;
                                // Swap
                                if (k != min)
                                {
                                    // Swap luminance
                                    swap = medianLum[min];
                                    medianLum[min] = medianLum[k];
                                    medianLum[k] = swap;
                                    // Swap rgbs
                                    for (l = 0; l < 3; ++l)
                                    {
                                        swap = medianBGR[l, min];
                                        medianBGR[l, min] = medianBGR[l, k];
                                        medianBGR[l, k] = swap;
                                    }
                                }
                            }
                            // Get the median
                            row[x] = (byte)(op0 * row[x] + op1 * medianBGR[0, 4]);
                            row[x + 1] = (byte)(op0 * row[x + 1] + op1 * medianBGR[1, 4]);
                            row[x + 2] = (byte)(op0 * row[x + 2] + op1 * medianBGR[2, 4]);
                        }
                        reporter?.Report(y, 1, h - 2);
                    }

                }
            }
            reporter?.Done();
        }
    }
}
