using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate right filter.
    /// </summary>
    [Filter]
    public sealed class RotateRightFilter : FilterBase
    {
        /// <inheritdoc/>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            Bitmap rotated = new(image.Height, image.Width);
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdRot = new(rotated, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3; // Width of a row
                int rotW = rotated.Width;
                int rotStride = bmdRot.Stride;
                int h = image.Height;
                int x, y;
                int idx;
                unsafe
                {
                    byte* rotStart = (byte*)bmdRot.Scan0;

                    // Iterate through rows
                    for (y = 0; y < h; y++)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (rotW - 1 - y) * bmd.Stride;
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                        {
                            idx = x / 3 * rotStride + y * 3; // Index in rotated image
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
