using System.Drawing;
using System.Drawing.Imaging;
using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Horizontal flip filter
    /// </summary>
    [Filter]
    public sealed class FlipHorizontalFilter : FilterInPlaceBase
    {
        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3; // Width of a row
                int wDiv2 = image.Width / 2 * 3; // Half the width
                int h = image.Height;
                int x, y;
                byte swap;
                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        // Iterate through columns
                        for (x = 0; x < wDiv2; x += 3)
                        {
                            swap = row[x]; // Blue
                            row[x] = row[wMul3 - x - 3];
                            row[wMul3 - x - 3] = swap;

                            swap = row[x + 1]; // Green
                            row[x + 1] = row[wMul3 - x - 2];
                            row[wMul3 - x - 2] = swap;

                            swap = row[x + 2]; // Red
                            row[x + 2] = row[wMul3 - x - 1];
                            row[wMul3 - x - 1] = swap;
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
