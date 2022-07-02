using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate 180 degrees filter
    /// </summary>
    [Filter]
    public sealed class Rotate180Filter : FilterInPlaceBase
    {
        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                int width_3 = image.Width * 3;
                int h = image.Height;
                int hDiv2 = h / 2;
                int x, y, stride = bmd.Stride;
                byte swap;
                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < hDiv2; ++y)
                    {
                        // Get rows
                        byte* row1 = (byte*)bmd.Scan0 + (y * stride);
                        byte* row2 = (byte*)bmd.Scan0 + ((h - y - 1) * stride);
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
