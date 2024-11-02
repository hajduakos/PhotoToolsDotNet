using FilterLib.IO;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace FilterLib.Tests
{
    static class Common
    {
        public static bool CheckFilter(string original, string expected, Filters.IFilter filter, int tolerance = 0)
        {
            string path = TestContext.CurrentContext.TestDirectory + "/TestImages/";

            Image bmpOriginal = new BitmapCodec().Read(path + original);
            Image bmpActual = filter.Apply(bmpOriginal);
            bool ok;
            if (File.Exists(path + expected))
            {
                Image bmpExpected = new BitmapCodec().Read(path + expected);
                ok = Compare(bmpActual, bmpExpected, tolerance);
            }
            else
            {
                ok = false;
            }
            if (!ok)
            {
                new BitmapCodec().Write(bmpActual, path + expected.Replace(".bmp", "_actual.bmp"));
            }
            return ok;
        }

        public static bool CheckBlend(string original1, string original2, string expected, Blending.IBlend blend, int tolerance = 0)
        {
            string path = TestContext.CurrentContext.TestDirectory + "/TestImages/";

            Image bmpOriginal1 = new BitmapCodec().Read(path + original1);
            Image bmpOriginal2 = new BitmapCodec().Read(path + original2);
            Image bmpActual = blend.Apply(bmpOriginal1, bmpOriginal2);
            bool ok;
            if (File.Exists(path + expected))
            {
                Image bmpExpected = new BitmapCodec().Read(path + expected);
                ok = Compare(bmpActual, bmpExpected, tolerance);
            }
            else
            {
                ok = false;
            }
            if (!ok)
            {
                new BitmapCodec().Write(bmpActual, path + expected.Replace(".bmp", "_actual.bmp"));
            }
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
