using FilterLib.Reporting;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Flip vertical filter.
    /// </summary>
    [Filter]
    public sealed class FlipVerticalFilter : FilterInPlaceBase
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
                    int h = image.Height;
                    int hDiv2 = h / 2; // Half the height
                    int x, y, stride = width_3;
                    byte swap;
                    // Iterate through rows
                    for (y = 0; y < hDiv2; ++y)
                    {
                        // Get rows
                        byte* row1 = start + (y * stride);
                        byte* row2 = start + ((h - y - 1) * stride);
                        // Iterate through columns
                        for (x = 0; x < width_3; ++x)
                        {
                            swap = row1[x];
                            row1[x] = row2[x];
                            row2[x] = swap;
                        }
                        reporter?.Report(y, 0, hDiv2 - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
