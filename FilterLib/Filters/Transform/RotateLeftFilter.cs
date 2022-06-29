using FilterLib.Reporting;
using FilterLib.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate left filter.
    /// </summary>
    [Filter]
    public sealed class RotateLeftFilter : FilterBase
    {
        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Rotated image
            Bitmap rotated = new(image.Height, image.Width);
            // Lock bits
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdRot = new(rotated, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3; // Width of a row
                int rotH = rotated.Height;
                int rotStride = bmdRot.Stride;
                int h = image.Height; // Image height
                int x, y;
                int idx;
                unsafe
                {
                    byte* rotStart = (byte*)bmdRot.Scan0;

                    // Iterate through rows
                    for (y = 0; y < h; y++)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                        {
                            idx = (rotH - x / 3 - 1) * rotStride + y * 3; // Index in rotated image
                            rotStart[idx] = row[x];
                            rotStart[idx + 1] = row[x + 1];
                            rotStart[idx + 2] = row[x + 2];
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
            return rotated;
        }
    }
}
