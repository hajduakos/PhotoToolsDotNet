using System.Drawing.Imaging;
using Bitmap = System.Drawing.Bitmap;
using Rectangle = System.Drawing.Rectangle;

namespace FilterLib.Util
{
    /// <summary>
    /// Utility class for adapting Bitmaps to Images
    /// </summary>
    public static class BitmapAdapter
    {
        /// <summary>
        /// Load a bitmap from file into image.
        /// </summary>
        /// <param name="path">Path to bitmap</param>
        /// <returns>Image with contents of the bitmap</returns>
        public static Image FromBitmapPath(string path)
        {
            using Bitmap bmp = new(path);
            Image img = new(bmp.Width, bmp.Height);
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int width_3 = bmp.Width * 3;
            unsafe
            {
                fixed (byte* imgStart = img)
                {
                    for (int y = 0; y < bmp.Height; ++y)
                    {
                        byte* bmpRow = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        byte* imgRow = imgStart + y * width_3;
                        for (int x = 0; x < width_3; x += 3)
                        {
                            imgRow[x] = bmpRow[x + 2];
                            imgRow[x + 1] = bmpRow[x + 1];
                            imgRow[x + 2] = bmpRow[x];
                        }
                    }
                }
            }
            bmp.UnlockBits(bmd);
            return img;
        }
    }
}
