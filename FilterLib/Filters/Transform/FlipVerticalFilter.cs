using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Flip vertical filter.
    /// </summary>
    [Filter]
    public sealed class FlipVerticalFilter : FilterInPlaceBase
    {
        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                int width_3 = image.Width * 3; // Width of a row
                int h = image.Height;
                int hDiv2 = h / 2; // Half the height
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
                        for (x = 0; x < width_3; ++x)
                        {
                            swap = row1[x];
                            row1[x] = row2[x];
                            row2[x] = swap;
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, hDiv2 - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
