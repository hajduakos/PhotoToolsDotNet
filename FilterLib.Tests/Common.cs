using FilterLib.Blending;
using FilterLib.Filters;
using FilterLib.Util;
using NUnit.Framework;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace FilterLib.Tests
{
    static class Common
    {
        public static bool CheckFilter(string original, string expected, IFilter filter, int tolerance = 0)
        {
            string path = TestContext.CurrentContext.TestDirectory + "/TestImages/";

            using Bitmap bmpOriginal = new(path + original);
            using Bitmap bmpActual = filter.Apply(bmpOriginal);
            using Bitmap bmpExpected = original == expected ? (Bitmap)bmpOriginal.Clone() : new Bitmap(path + expected);
            bool ok = Compare(bmpActual, bmpExpected, tolerance);
            if (!ok) bmpActual.Save(path + expected.Replace(".bmp", "") + "_actual.bmp", ImageFormat.Bmp);
            return ok;
        }

        public static bool CheckBlend(string original1, string original2, string expected, IBlend blend, int tolerance = 0)
        {
            string path = TestContext.CurrentContext.TestDirectory + "/TestImages/";

            using Bitmap bmpOriginal1 = new(path + original1);
            using Bitmap bmpOriginal2 = original1 == original2 ? (Bitmap)bmpOriginal1.Clone() : new Bitmap(path + original2);
            using Bitmap bmpActual = blend.Apply(bmpOriginal1, bmpOriginal2);
            using Bitmap bmpExpected = original1 == expected ? (Bitmap)bmpOriginal1.Clone() :
                (original2 == expected ? (Bitmap)bmpOriginal2.Clone() : new Bitmap(path + expected));
            bool ok = Compare(bmpActual, bmpExpected, tolerance);
            if (!ok) bmpActual.Save(path + expected.Replace(".bmp", "") + "_actual.bmp");
            return ok;
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

        public static int ParamCount(Type type) => ReflectiveApi.GetFilterProperties(type).ToArray().Length;
    }
}
