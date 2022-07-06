using FilterLib.Reporting;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate right filter.
    /// </summary>
    [Filter]
    public sealed class RotateRightFilter : FilterBase
    {
        /// <inheritdoc/>
        public override Image Apply(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            Image rotated = new(image.Height, image.Width);
            unsafe
            {
                fixed (byte* start = image, rotStart = rotated)
                {
                    int width_3 = image.Width * 3; // Width of a row
                    int rotW = rotated.Width;
                    int rotStride = rotW * 3;
                    int h = image.Height;
                    int x, y;
                    int idx;

                    // Iterate through rows
                    for (y = 0; y < h; y++)
                    {
                        // Get row
                        byte* row = start + (rotW - 1 - y) * width_3;
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            idx = x / 3 * rotStride + y * 3; // Index in rotated image
                            rotStart[idx] = row[x];
                            rotStart[idx + 1] = row[x + 1];
                            rotStart[idx + 2] = row[x + 2];
                        }
                        reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
            return rotated;
        }
    }
}
