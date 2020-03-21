using System.Drawing;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using IReporter = FilterLib.Reporting.IReporter;
using FilterLib.Util;

namespace FilterLib
{
    public abstract class PerPixelFilterBase : FilterInPlaceBase
    {
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            ApplyStart();

            using (DisposableBitmapData bmd = new DisposableBitmapData(image, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3; // Width of a row
                int h = image.Height; // Image height
                int x, y;

                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                            ProcessPixel(row + x + 2, row + x + 1, row + x);
                        if (reporter != null && ((y & 63) == 0)) reporter.Report(y, 0, h - 1);
                    }
                }
            }

            ApplyEnd();
        }

        protected virtual void ApplyStart() { }

        protected abstract unsafe void ProcessPixel(byte* r, byte* g, byte* b);

        protected virtual void ApplyEnd() { }
    }
}
