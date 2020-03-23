using FilterLib.Reporting;
using FilterLib.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate 180 degrees filter
    /// </summary>
    [Filter]
    public sealed class Rotate180Filter : FilterInPlaceBase
    {
        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new DisposableBitmapData(image, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3;
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
                        for (x = 0; x < wMul3; x += 3)
                        {
                            swap = row1[x]; // Blue
                            row1[x] = row2[wMul3 - x - 3];
                            row2[wMul3 - x - 3] = swap;

                            swap = row1[x + 1]; // Green
                            row1[x + 1] = row2[wMul3 - x - 2];
                            row2[wMul3 - x - 2] = swap;

                            swap = row1[x + 2]; // Red
                            row1[x + 2] = row2[wMul3 - x - 1];
                            row2[wMul3 - x - 1] = swap;
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, hDiv2 - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
