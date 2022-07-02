using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterScript.Test
{
    static class Program
    {
        static int Main(string[] args)
        {
            using Bitmap expected = new(args[0]);
            using Bitmap actual = new(args[1]);
            if (Compare(actual, expected, 3)) return 0;
            return 1;
        }

        private static bool Compare(Bitmap actual, Bitmap expected, int tolerance)
        {
            if (actual.Width != expected.Width) return false;
            if (actual.Height != expected.Height) return false;
            using DisposableBitmapData bmdAct = new(actual, PixelFormat.Format24bppRgb);
            using DisposableBitmapData bmdExp = new(expected, PixelFormat.Format24bppRgb);
            int width_3 = actual.Width * 3;
            int h = actual.Height;
            int x, y;
            unsafe
            {
                for (y = 0; y < h; ++y)
                {
                    // Get row
                    byte* rowAct = (byte*)bmdAct.Scan0 + (y * bmdAct.Stride);
                    byte* rowExp = (byte*)bmdExp.Scan0 + (y * bmdExp.Stride);
                    // Iterate through columns
                    for (x = 0; x < width_3; ++x)
                        if (Math.Abs(rowAct[x] - rowExp[x]) > tolerance)
                            return false;
                }
            }
            return true;
        }
    }
}
