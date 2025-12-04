using FilterLib.Reporting;
using FilterLib.Util;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Artistic
{
    [Filter("Create oil paint effect by replacing each pixel with the most frequent intensity in a given radius.")]
    public sealed class OilPaintFilter : FilterInPlaceBase
    {
        private int radius;
        private int intensityLevels;

        /// <summary>
        /// Brush radius [0;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int Radius
        {
            get { return radius; }
            set { radius = System.Math.Max(0, value); }
        }

        /// <summary>
        /// Number of intensity levels [0;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(255)]
        public int IntensityLevels
        {
            get { return intensityLevels; }
            set { intensityLevels = value.Clamp(0, 255); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="radius">Brush radius [0;...]</param>
        /// <param name="intensityLevels">Number of intensity levels [0;255]</param>
        public OilPaintFilter(int radius = 0, int intensityLevels = 255)
        {
            Radius = radius;
            IntensityLevels = intensityLevels;
        }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(image.Width == original.Width);
            int width_3 = image.Width * 3;
            int radius_3 = radius * 3;
            float iMult = intensityLevels / 255f;
            // For each pixel we calculate the frequency of each intensity level within the radius,
            // and also the average R, G and B values corresponding to the intensity levels. Then
            // we replace the current pixel with the R, G, B values corresponding to the most
            // frequent intensity level (mode). To make this more efficient, we go row-by-row and
            // use a moving window:
            // - First we calculate the full window for the first pixel
            // - But then in each step moving right, we drop one row from the left and add one at the right
            fixed (byte* newStart = image, oldStart = original)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                Parallel.For(0, image.Height, y =>
                {
                    byte* newRow = newStart0 + y * width_3;
                    byte* oldRow = oldStart0 + y * width_3;

                    int[] red = new int[256];
                    int[] green = new int[256];
                    int[] blue = new int[256];
                    int[] intensities = new int[256];

                    for (int i = 0; i < 256; ++i) red[i] = green[i] = blue[i] = intensities[i] = 0;

                    // Calculate full window around first column of current row
                    for (int xSub = 0; xSub <= radius && xSub < image.Width; ++xSub)
                    {
                        for (int ySub = y < radius ? -y : -radius; y + ySub < image.Height && ySub <= radius; ++ySub)
                        {
                            int idx = ySub * width_3 + xSub * 3;
                            int lum = (int)(RGB.GetLuminance(oldRow[idx], oldRow[idx + 1], oldRow[idx + 2]) * iMult);
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
                                int lum = (int)(RGB.GetLuminance(oldRow[idx], oldRow[idx + 1], oldRow[idx + 2]) * iMult);
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
                                int lum = (int)(RGB.GetLuminance(oldRow[idx], oldRow[idx + 1], oldRow[idx + 2]) * iMult);
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
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }
            reporter?.Done();
        }
    }
}
