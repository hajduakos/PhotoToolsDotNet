using FilterLib.Reporting;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Horizontal flip filter
    /// </summary>
    [Filter]
    public sealed class FlipHorizontalFilter : FilterInPlaceBase
    {
        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();

            unsafe
            {
                fixed (byte* start = image)
                {
                    int width_3 = image.Width * 3; // Width of a row
                    int wDiv2 = image.Width / 2 * 3; // Half the width
                    int h = image.Height;
                    int x, y;
                    byte swap;
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get row
                        byte* row = start + (y * width_3);
                        // Iterate through columns
                        for (x = 0; x < wDiv2; x += 3)
                        {
                            swap = row[x]; // Blue
                            row[x] = row[width_3 - x - 3];
                            row[width_3 - x - 3] = swap;

                            swap = row[x + 1]; // Green
                            row[x + 1] = row[width_3 - x - 2];
                            row[width_3 - x - 2] = swap;

                            swap = row[x + 2]; // Red
                            row[x + 2] = row[width_3 - x - 1];
                            row[width_3 - x - 1] = swap;
                        }
                        reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
