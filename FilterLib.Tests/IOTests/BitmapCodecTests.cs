using FilterLib.IO;
using NUnit.Framework;

namespace FilterLib.Tests.IOTests
{
    public class BitmapCodecTests
    {
        [Test]
        public void TestWrite()
        {
            Image img = new(2, 2);
            img[0, 0, 2] = 255; // Red
            img[1, 0, 1] = 255; // Green
            img[0, 1, 0] = 255; // Blue
            img[1, 1, 0] = img[1, 1, 1] = img[1, 1, 2] = 255; // White
            byte[] bmp = new BitmapCodec().WriteArray(img);
            // BMP header
            Assert.AreEqual(new byte[] {
                0x42, 0x4D,
                0x46, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x36, 0x00, 0x00, 0x00,
            }, bmp[0..14]);
            // DIB header
            Assert.AreEqual(new byte[]
            {
                0x28, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x00, 0x00,
                0x01, 0x00,
                0x18, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x10, 0x00, 0x00, 0x00,
                0x13, 0x0B, 0x00, 0x00,
                0x13, 0x0B, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
            }, bmp[14..54]);
            // Pixel array
            Assert.AreEqual(new byte[]
            {
                0x00, 0x00, 0xFF,
                0xFF, 0xFF, 0xFF,
                0x00, 0x00,
                0xFF, 0x00, 0x00,
                0x00, 0xFF, 0x00,
                0x00, 0x00
            }, bmp[54..]);
        }
    }
}
