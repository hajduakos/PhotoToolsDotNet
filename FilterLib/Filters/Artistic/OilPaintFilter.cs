using FilterLib.Reporting;

namespace FilterLib.Filters.Artistic
{
    /// <summary>
    /// Oil paint filter.
    /// </summary>
    [Filter]
    public sealed class OilPaintFilter : FilterInPlaceBase
    {
        private int radius;

        /// <summary>
        /// Brush radius [1;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(1)]
        public int Radius
        {
            get { return radius; }
            set { radius = System.Math.Max(1, value); }
        }

        /// <summary>
        /// Constructor with brush radius.
        /// </summary>
        /// <param name="radius">Brush radius</param>
        public OilPaintFilter(int radius = 1) => Radius = radius;

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
                    int radius_3 = radius * 3;
                    int[] red = new int[256];
                    int[] green = new int[256];
                    int[] blue = new int[256];
                    int[] intensities = new int[256];

                    // Iterate through rows
                    for (int y = 0; y < image.Height; ++y)
                    {
                        // Get rows
                        byte* row = start + (y * width_3);
                        byte* rowOrig = origStart + (y * width_3);

                        // Clear arrays
                        for (int xSub = 0; xSub < 256; ++xSub) red[xSub] = green[xSub] = blue[xSub] = intensities[xSub] = 0;

                        // Build up arrays with the surrounding of the first column of the actual row
                        // Horizontal
                        for (int xSub = 0; xSub <= radius && xSub < image.Width; ++xSub)
                        {
                            // Vertically from -radius to +radius
                            for (int ySub = y < radius ? -y : -radius; y + ySub < image.Height && ySub <= radius; ++ySub)
                            {
                                // Calculate index (relative to current row)
                                int idx = ySub * width_3 + xSub * 3;
                                // Get luminance
                                int avg = (int)(0.299 * rowOrig[idx] + 0.587 * rowOrig[idx + 1] + 0.114 * rowOrig[idx + 2]);
                                // Increase values
                                intensities[avg]++;
                                // Sum values
                                red[avg] += rowOrig[idx];
                                green[avg] += rowOrig[idx + 1];
                                blue[avg] += rowOrig[idx + 2];
                            }
                        }
                        // Get the most frequent intensity
                        int max = 0;
                        for (int xSub = 0; xSub < 256; xSub++) if (intensities[max] < intensities[xSub]) max = xSub;
                        // First element of the row will be the average of the maximal intensity
                        row[0] = (byte)(red[max] / intensities[max]);
                        row[1] = (byte)(green[max] / intensities[max]);
                        row[2] = (byte)(blue[max] / intensities[max]);

                        // Iterate through other columns
                        // Rather than building up the whole array again, we just take in another column
                        // (+radius) and remove a column (-radius-1)
                        for (int x = 3; x < width_3; x += 3)
                        {
                            // If we can remove a column
                            if (x / 3 - radius - 1 >= 0)
                            {
                                // Remove column (vertically from -radius to +radius)
                                for (int ySub = y < radius ? -y : -radius; y + ySub < image.Height && ySub <= radius; ++ySub)
                                {
                                    // Calculate index (relative to current row)
                                    int idx = ySub * width_3 + x - radius_3 - 3;
                                    // Get luminance
                                    int avg = (int)(0.299 * rowOrig[idx] + 0.587 * rowOrig[idx + 1] + 0.114 * rowOrig[idx + 2]);
                                    // Decrease values
                                    intensities[avg]--;
                                    // Sum values
                                    red[avg] -= rowOrig[idx];
                                    green[avg] -= rowOrig[idx + 1];
                                    blue[avg] -= rowOrig[idx + 2];
                                }
                            }
                            // If we can add a new column
                            if (x / 3 + radius < image.Width)
                            {
                                // Add column (vertically from -radius to +radius)
                                for (int ySub = y < radius ? -y : -radius; y + ySub < image.Height && ySub <= radius; ++ySub)
                                {
                                    // Calculate index (relative to current row)
                                    int idx = ySub * width_3 + x + radius_3;
                                    // Get luminance
                                    int avg = (int)(0.299 * rowOrig[idx] + 0.587 * rowOrig[idx + 1] + 0.114 * rowOrig[idx + 2]);
                                    // Increase values
                                    intensities[avg]++;
                                    // Sum values
                                    red[avg] += rowOrig[idx];
                                    green[avg] += rowOrig[idx + 1];
                                    blue[avg] += rowOrig[idx + 2];
                                }
                            }
                            // Get the most frequent intensity
                            max = 0;
                            for (int xSub = 0; xSub < 256; xSub++) if (intensities[max] < intensities[xSub]) max = xSub;
                            // Actual element will be the average of the maximal intensity
                            row[x] = (byte)(red[max] / intensities[max]);
                            row[x + 1] = (byte)(green[max] / intensities[max]);
                            row[x + 2] = (byte)(blue[max] / intensities[max]);
                        }

                        reporter?.Report(y, 0, image.Height - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
