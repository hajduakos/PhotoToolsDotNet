using FilterLib.IO;
using NUnit.Framework;
using System.IO;

namespace FilterLib.Tests.IOTests
{
    public class BitmapCodecTests
    {
        private byte[] CreateImage()
        {
            Image img = new(2, 2);
            img[0, 0, 2] = 255; // Red
            img[1, 0, 1] = 255; // Green
            img[0, 1, 0] = 255; // Blue
            img[1, 1, 0] = img[1, 1, 1] = img[1, 1, 2] = 255; // White
            return new BitmapCodec().Write(img);
        }

        [Test]
        public void TestWrite()
        {
            byte[] bmp = CreateImage();
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
                0x00, 0x00,
            }, bmp[54..]);
        }

        [Test]
        public void TestReadShortBMPHeader()
        {
            Assert.Throws<EndOfStreamException>(() => new BitmapCodec().Read(CreateImage()[0..2]));
        }

        [Test]
        public void TestReadInvalidBMPHeader()
        {
            byte[] data = CreateImage();
            data[0] = 0x00;
            Assert.Throws<CodecException>(() => new BitmapCodec().Read(data));
            data = CreateImage();
            data[1] = 0x00;
            Assert.Throws<CodecException>(() => new BitmapCodec().Read(data));
        }

        [Test]
        public void TestReadShortDIBHeader()
        {
            Assert.Throws<EndOfStreamException>(() => new BitmapCodec().Read(CreateImage()[0..20]));
        }

        [Test]
        public void TestReadInvalidDIBHeaderSize()
        {
            byte[] data = CreateImage();
            data[15] = 0x12;
            Assert.Throws<CodecException>(() => new BitmapCodec().Read(data));
        }

        [Test]
        public void TestReadUnsupportedColorPlanes()
        {
            byte[] data = CreateImage();
            data[26] = 0x12;
            Assert.Throws<CodecException>(() => new BitmapCodec().Read(data));
        }

        [Test]
        public void TestReadUnsupportedBPP()
        {
            byte[] data = CreateImage();
            data[28] = 0x20;
            Assert.Throws<CodecException>(() => new BitmapCodec().Read(data));
        }

        [Test]
        public void TestReadUnsupportedCompression()
        {
            byte[] data = CreateImage();
            data[30] = 0x12;
            Assert.Throws<CodecException>(() => new BitmapCodec().Read(data));
        }

        [Test]
        public void TestReadUnsupportedColorPalettes()
        {
            byte[] data = CreateImage();
            data[46] = 0x56;
            Assert.Throws<CodecException>(() => new BitmapCodec().Read(data));
        }

        [Test]
        public void TestReadUnsupportedImportantColors()
        {
            byte[] data = CreateImage();
            data[50] = 0x12;
            Assert.Throws<CodecException>(() => new BitmapCodec().Read(data));
        }

        [Test]
        public void TestRead()
        {
            Image img = new BitmapCodec().Read(new byte[]
            {
                // BMP header
                0x42, 0x4D,
                0x46, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x36, 0x00, 0x00, 0x00,
                // DIB header
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
                // Pixel array
                0x00, 0x00, 0xFF,
                0xFF, 0xFF, 0xFF,
                0x00, 0x00,
                0xFF, 0x00, 0x00,
                0x00, 0xFF, 0x00,
                0x00, 0x00,
            });
            Assert.AreEqual(2, img.Width);
            Assert.AreEqual(2, img.Height);
            Assert.AreEqual(255, img[0, 0, 2]);
            Assert.AreEqual(255, img[1, 0, 1]);
            Assert.AreEqual(255, img[0, 1, 0]);
            Assert.AreEqual(255, img[1, 1, 0]);
            Assert.AreEqual(255, img[1, 1, 1]);
            Assert.AreEqual(255, img[1, 1, 2]);
        }

        [Test]
        public void TestWriteAndReadFile()
        {
            string path = TestContext.CurrentContext.TestDirectory + "/TestImages/__test_write.bmp";
            BitmapCodec codec = new();
            Image img = new(10, 20);
            img[2, 5, 1] = 123;
            codec.Write(img, path);
            Image img2 = codec.Read(path);
            Assert.AreEqual(img.Width, img2.Width);
            Assert.AreEqual(img.Height, img2.Height);
            Assert.AreEqual(img[2, 5, 1], img2[2, 5, 1]);
        }
    }
}
