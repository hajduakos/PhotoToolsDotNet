using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterLib.Util
{
    /// <summary>
    /// Wraps a BitmapData into an automatically disposable interface.
    /// </summary>
    public sealed class DisposableBitmapData : IDisposable
    {
        private Bitmap image;
        private readonly BitmapData bitmapData;

        public DisposableBitmapData(Bitmap image, PixelFormat format)
            : this(image, new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, format) { }

        public DisposableBitmapData(Bitmap image, ImageLockMode flags, PixelFormat format)
            : this(image, new Rectangle(0, 0, image.Width, image.Height), flags, format) { }

        public DisposableBitmapData(Bitmap image, Rectangle rect, ImageLockMode flags, PixelFormat format)
        {
            this.image = image;
            bitmapData = image.LockBits(rect, flags, format);
        }

        public void Dispose()
        {
            image.UnlockBits(bitmapData);
            this.image = null;
        }

        public int Height
        {
            get { return bitmapData.Height; }
            set { bitmapData.Height = value; }
        }

        public PixelFormat PixelFormat
        {
            get { return bitmapData.PixelFormat; }
            set { bitmapData.PixelFormat = value; }
        }

        public IntPtr Scan0
        {
            get { return bitmapData.Scan0; }
            set { bitmapData.Scan0 = value; }
        }

        public int Stride
        {
            get { return bitmapData.Stride; }
            set { bitmapData.Stride = value; }
        }

        public int Width
        {
            get { return bitmapData.Width; }
            set { bitmapData.Width = value; }

        }

    }
}
