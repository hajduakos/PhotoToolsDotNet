using FilterLib.Reporting;

namespace FilterLib.Filters.Artistic
{
    /// <summary>
    /// Create an oil paint effect by replacing each pixel with the most
    /// frequent intesity (mode) within a given radius.
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
        /// Constructor.
        /// </summary>
        /// <param name="radius">Brush radius [1;...]</param>
        public OilPaintFilter(int radius = 1) => Radius = radius;

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(image.Width == original.Width);
            int width_3 = image.Width * 3;
            int radius_3 = radius * 3;
            int[] red = new int[256];
            int[] green = new int[256];
            int[] blue = new int[256];
            int[] intensities = new int[256];
            // For each pixel we calculate the frequency of each intensity level within the radius,
            // and also the average R, G and B values corresponding to the intensity levels. Then
            // we replace the current pixel with the R, G, B values corresponding to the most
            // frequent intensity level (mode). To make this more efficient, we go row-by-row and
            // use a moving window:
            // - First we calculate the full window for the first pixel
            // - But then in each step moving right, we drop one row from the left and add one at the right
            fixed (byte* newStart = image, oldStart = original)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    byte* newRow = newStart + y * width_3;
                    byte* oldRow = oldStart + y * width_3;

                    for (int i = 0; i < 256; ++i) red[i] = green[i] = blue[i] = intensities[i] = 0;

                    // Calculate full window around first column of current row
                    for (int xSub = 0; xSub <= radius && xSub < image.Width; ++xSub)
                    {
                        for (int ySub = y < radius ? -y : -radius; y + ySub < image.Height && ySub <= radius; ++ySub)
                        {
                            int idx = ySub * width_3 + xSub * 3;
                            int lum = (int)Util.RGB.GetLuminance(oldRow[idx], oldRow[idx + 1], oldRow[idx + 2]);
                            ++intensities[lum];
                            red[lum] += oldRow[idx];
                            green[lum] += oldRow[idx + 1];
                            blue[lum] += oldRow[idx + 2];
                        }
                    }
                    // Calculate first column of current row
                    int max = 0;
                    for (int i = 0; i < 256; i++) if (intensities[max] < intensities[i]) max = i;
                    newRow[0] = (byte)(red[max] / intensities[max]);
                    newRow[1] = (byte)(green[max] / intensities[max]);
                    newRow[2] = (byte)(blue[max] / intensities[max]);

                    // Iterate through other columns and update window
                    for (int x = 3; x < width_3; x += 3)
                    {
                        // Remove a column from the left
                        if (x / 3 - radius - 1 >= 0)
                        {
                            for (int ySub = y < radius ? -y : -radius; y + ySub < image.Height && ySub <= radius; ++ySub)
                            {
                                int idx = ySub * width_3 + x - radius_3 - 3;
                                int lum = (int)Util.RGB.GetLuminance(oldRow[idx], oldRow[idx + 1], oldRow[idx + 2]);
                                --intensities[lum];
                                red[lum] -= oldRow[idx];
                                green[lum] -= oldRow[idx + 1];
                                blue[lum] -= oldRow[idx + 2];
                            }
                        }
                        // Add column to right
                        if (x / 3 + radius < image.Width)
                        {
                            for (int ySub = y < radius ? -y : -radius; y + ySub < image.Height && ySub <= radius; ++ySub)
                            {
                                int idx = ySub * width_3 + x + radius_3;
                                int lum = (int)Util.RGB.GetLuminance(oldRow[idx], oldRow[idx + 1], oldRow[idx + 2]);
                                ++intensities[lum];
                                red[lum] += oldRow[idx];
                                green[lum] += oldRow[idx + 1];
                                blue[lum] += oldRow[idx + 2];
                            }
                        }
                        // Calculate current column of current row
                        max = 0;
                        for (int i = 0; i < 256; i++) if (intensities[max] < intensities[i]) max = i;
                        newRow[x] = (byte)(red[max] / intensities[max]);
                        newRow[x + 1] = (byte)(green[max] / intensities[max]);
                        newRow[x + 2] = (byte)(blue[max] / intensities[max]);
                    }
                    reporter?.Report(y, 0, image.Height - 1);
                }
            }
            reporter?.Done();
        }
    }
}
