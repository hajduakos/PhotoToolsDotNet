using FilterLib.Reporting;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate 180 degrees filter
    /// </summary>
    [Filter]
    public sealed class Rotate180Filter : FilterInPlaceBase
    {
        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();

            unsafe
            {
                fixed (byte* start = image)
                {
                    int width_3 = image.Width * 3;
                    int h = image.Height;
                    int hDiv2 = h / 2;
                    int x, y, stride = width_3;
                    byte swap;
                    // Iterate through rows
                    for (y = 0; y < hDiv2; ++y)
                    {
                        // Get rows
                        byte* row1 = start + (y * stride);
                        byte* row2 = start + ((h - y - 1) * stride);
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            swap = row1[x]; // Blue
                            row1[x] = row2[width_3 - x - 3];
                            row2[width_3 - x - 3] = swap;

                            swap = row1[x + 1]; // Green
                            row1[x + 1] = row2[width_3 - x - 2];
                            row2[width_3 - x - 2] = swap;

                            swap = row1[x + 2]; // Red
                            row1[x + 2] = row2[width_3 - x - 1];
                            row2[width_3 - x - 1] = swap;
                        }
                        reporter?.Report(y, 0, hDiv2 - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
