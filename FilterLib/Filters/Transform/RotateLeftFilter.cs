using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate left filter.
    /// </summary>
    [Filter]
    public sealed class RotateLeftFilter : FilterBase
    {
        /// <inheritdoc/>
        public override Image Apply(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Rotated image
            Image rotated = new(image.Height, image.Width);
            // Lock bits
            unsafe
            {
                fixed (byte* start = image, rotStart = rotated)
                {
                    int width_3 = image.Width * 3; // Width of a row
                    int rotH = rotated.Height;
                    int rotStride = rotated.Width * 3;
                    int h = image.Height; // Image height
                    int x, y;
                    int idx;

                    // Iterate through rows
                    for (y = 0; y < h; y++)
                    {
                        // Get row
                        byte* row = start + (y * width_3);
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            idx = (rotH - x / 3 - 1) * rotStride + y * 3; // Index in rotated image
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
