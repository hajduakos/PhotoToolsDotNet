using FilterLib.Util;
using NUnit.Framework;
using System.Linq;
using Bitmap = System.Drawing.Bitmap;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Tests
{
    static class Common
    {
        public static bool CheckFilter(string original, string expected, Filters.IFilter filter, int tolerance = 0)
        {
            string path = TestContext.CurrentContext.TestDirectory + "/TestImages/";

            Image bmpOriginal = BitmapAdapter.FromBitmapPath(path + original);
            Image bmpActual = filter.Apply(bmpOriginal);
            Image bmpExpected = original == expected ? (Image)bmpOriginal.Clone() : BitmapAdapter.FromBitmapPath(path + expected);
            bool ok = Compare(bmpActual, bmpExpected, tolerance);
            if (!ok) BitmapAdapter.ToBitmap(bmpActual).Save(path + expected.Replace(".bmp", "_actual.bmp"), ImageFormat.Bmp);
            return ok;
        }

        public static bool CheckBlend(string original1, string original2, string expected, Blending.IBlend blend, int tolerance = 0)
        {
            string path = TestContext.CurrentContext.TestDirectory + "/TestImages/";

            Image bmpOriginal1 = BitmapAdapter.FromBitmapPath(path + original1);
            Image bmpOriginal2 = original1 == original2 ? (Image)bmpOriginal1.Clone() : BitmapAdapter.FromBitmapPath(path + original2);
            Image bmpActual = blend.Apply(bmpOriginal1, bmpOriginal2);
            Image bmpExpected = original1 == expected ? (Image)bmpOriginal1.Clone() :
                (original2 == expected ? (Image)bmpOriginal2.Clone() : BitmapAdapter.FromBitmapPath(path + expected));
            bool ok = Compare(bmpActual, bmpExpected, tolerance);
            if (!ok) BitmapAdapter.ToBitmap(bmpActual).Save(path + expected.Replace(".bmp", "_actual.bmp"));
            return ok;
        }

        private static bool Compare(Image actual, Image expected, int tolerance)
        {
            if (actual.Width != expected.Width) return false;
            if (actual.Height != expected.Height) return false;
            unsafe
            {
                fixed (byte* actStart = actual, expStart = expected)
                {
                    int width_3 = actual.Width * 3;
                    for (int y = 0; y < actual.Height; ++y)
                    {
                        byte* rowAct = actStart + (y * width_3);
                        byte* rowExp = expStart + (y * width_3);
                        for (int x = 0; x < width_3; ++x)
                            if (System.Math.Abs(rowAct[x] - rowExp[x]) > tolerance)
                                return false;
                    }
                }
                return true;
            }
        }

        public static int ParamCount(System.Type type) => ReflectiveApi.GetFilterProperties(type).Count();
    }
}
