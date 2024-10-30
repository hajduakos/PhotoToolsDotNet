using FilterLib;
using FilterLib.IO;

namespace FilterScript.Test
{
    static class Program
    {
        static int Main(string[] args)
        {
            Image expected = new BitmapCodec().Read(args[0]);
            Image actual = new BitmapCodec().Read(args[1]);
            if (Compare(actual, expected, 3)) return 0;
            return 1;
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
    }
}
