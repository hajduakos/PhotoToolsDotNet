using NUnit.Framework;
using FilterLib.Util;
using System;

namespace FilterLib.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class UtilTests
    {
        [Test]
        [TestCase(0, (-1000))]
        [TestCase(0, (-1))]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(25, 25)]
        [TestCase(127, 127)]
        [TestCase(254, 254)]
        [TestCase(255, 255)]
        [TestCase(255, 256)]
        [TestCase(255, 1000)]
        public void TestClamp(int expected, int actual) => Assert.AreEqual(expected, actual.Clamp(0, 255));

        [Test]
        [TestCase(0, (-1000))]
        [TestCase(0, (-1))]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(25, 25)]
        [TestCase(127, 127)]
        [TestCase(254, 254)]
        [TestCase(255, 255)]
        [TestCase(255, 256)]
        [TestCase(255, 1000)]
        public void TestClampToByteInt(byte expected, int actual) => Assert.AreEqual(expected, actual.ClampToByte());

        [Test]
        [TestCase(0, (-1000.1f))]
        [TestCase(0, (-1.1f))]
        [TestCase(0, 0.2f)]
        [TestCase(0, 0.5f)]
        [TestCase(1, 1.1f)]
        [TestCase(25, 25.1f)]
        [TestCase(127, 127.1f)]
        [TestCase(254, 254.9f)]
        [TestCase(255, 255.1f)]
        [TestCase(255, 256.1f)]
        [TestCase(255, 1000.1f)]
        public void TestClampToByteFloat(byte expected, float actual) => Assert.AreEqual(expected, actual.ClampToByte());

        [Test]
        public void TestRGB()
        {
            Assert.AreEqual(20, new RGB(20, 10, 50).R);
            Assert.AreEqual(10, new RGB(20, 10, 50).G);
            Assert.AreEqual(50, new RGB(20, 10, 50).B);
            Assert.AreEqual(new RGB(0, 255, 255), new RGB(-10, 270, 1000));
        }

        [Test]
        public void TestHSL()
        {
            Assert.AreEqual(20, new HSL(20, 10, 50).H);
            Assert.AreEqual(10, new HSL(20, 10, 50).S);
            Assert.AreEqual(50, new HSL(20, 10, 50).L);
            Assert.AreEqual(new HSL(10, 100, 0), new HSL(370, 200, -10));
        }

        [Test]
        public void TestRGBtoHSL()
        {
            Assert.AreEqual(new HSL(0, 0, 0), new RGB(0, 0, 0).ToHSL());
            Assert.AreEqual(new HSL(0, 100, 50), new RGB(255, 0, 0).ToHSL());
            Assert.AreEqual(new HSL(120, 100, 50), new RGB(0, 255, 0).ToHSL());
            Assert.AreEqual(new HSL(240, 100, 50), new RGB(0, 0, 255).ToHSL());
            Assert.AreEqual(new HSL(210, 25, 15), new RGB(30, 40, 50).ToHSL());
            Assert.AreEqual(new HSL(0, 0, 100), new RGB(255, 255, 255).ToHSL());
        }

        [Test]
        public void TestHSLtoRGB()
        {
            Assert.AreEqual(new RGB(0, 0, 0), new HSL(0, 0, 0).ToRGB());
            Assert.AreEqual(new RGB(255, 0, 0), new HSL(0, 100, 50).ToRGB());
            Assert.AreEqual(new RGB(0, 255, 0), new HSL(120, 100, 50).ToRGB());
            Assert.AreEqual(new RGB(0, 0, 255), new HSL(240, 100, 50).ToRGB());
            Assert.AreEqual(new RGB(28, 38, 47), new HSL(210, 25, 15).ToRGB());
            Assert.AreEqual(new RGB(255, 255, 255), new HSL(0, 0, 100).ToRGB());
        }

        [Test]
        public void TestConv3x3Parse()
        {
            Conv3x3 c = new("[1 2 3 ; 4 5 6 ; 7 8 9] / 10 + 11");
            Assert.AreEqual(1, c[0, 0]);
            Assert.AreEqual(2, c[1, 0]);
            Assert.AreEqual(3, c[2, 0]);
            Assert.AreEqual(4, c[0, 1]);
            Assert.AreEqual(5, c[1, 1]);
            Assert.AreEqual(6, c[2, 1]);
            Assert.AreEqual(7, c[0, 2]);
            Assert.AreEqual(8, c[1, 2]);
            Assert.AreEqual(9, c[2, 2]);
            Assert.AreEqual(10, c.Divisor);
            Assert.AreEqual(11, c.Bias);
        }

        [Test]
        public void TestGradientOf2()
        {
            Gradient g = new(new RGB(255, 100, 0), new RGB(0, 50, 255));
            Assert.AreEqual(new RGB(255, 100, 0), g.GetColor(0f));
            Assert.AreEqual(new RGB(127, 75, 127), g.GetColor(0.5f));
            Assert.AreEqual(new RGB(51, 60, 204), g.GetColor(0.8f));
            Assert.AreEqual(new RGB(0, 50, 255), g.GetColor(1f));
        }

        [Test]
        public void TestGradientOf3()
        {
            Gradient g = new(new RGB(255, 0, 0), new RGB(255, 255, 0), new RGB(0, 255, 0));
            Assert.AreEqual(new RGB(255, 0, 0), g.GetColor(0f));
            Assert.AreEqual(new RGB(255, 127, 0), g.GetColor(0.25f));
            Assert.AreEqual(new RGB(255, 255, 0), g.GetColor(0.5f));
            Assert.AreEqual(new RGB(127, 255, 0), g.GetColor(0.75f));
            Assert.AreEqual(new RGB(0, 255, 0), g.GetColor(1f));
        }

        [Test]
        public void TestGradientParseOf2()
        {
            Gradient g = new("0 (255 100 0), 1 (0 50 255)");
            Assert.AreEqual(new RGB(255, 100, 0), g.GetColor(0f));
            Assert.AreEqual(new RGB(127, 75, 127), g.GetColor(0.5f));
            Assert.AreEqual(new RGB(51, 60, 204), g.GetColor(0.8f));
            Assert.AreEqual(new RGB(0, 50, 255), g.GetColor(1f));
        }

        [Test]
        public void TestGradientParseOf3()
        {
            Gradient g = new("0 (255 0 0), 0.5 (255 255 0), 1 (0 255 0)");
            Assert.AreEqual(new RGB(255, 0, 0), g.GetColor(0f));
            Assert.AreEqual(new RGB(255, 127, 0), g.GetColor(0.25f));
            Assert.AreEqual(new RGB(255, 255, 0), g.GetColor(0.5f));
            Assert.AreEqual(new RGB(127, 255, 0), g.GetColor(0.75f));
            Assert.AreEqual(new RGB(0, 255, 0), g.GetColor(1f));
        }

        [Test]
        public void TestSize()
        {
            Assert.AreEqual(25, Size.Absolute(25).ToAbsolute(0));
            Assert.AreEqual(0, Size.Relative(.25f).ToAbsolute(0));
            Assert.AreEqual(25, Size.Relative(.25f).ToAbsolute(100));
            Assert.AreEqual(200, Size.Relative(2f).ToAbsolute(100));
        }

        [Test]
        public void TestSizeParse()
        {
            Assert.AreEqual(25, Size.FromString("25px").ToAbsolute(0));
            Assert.AreEqual(0, Size.FromString("25%").ToAbsolute(0));
            Assert.AreEqual(25, Size.FromString("25 %").ToAbsolute(100));
            Assert.AreEqual(200, Size.FromString("200   %").ToAbsolute(100));

            Assert.Throws<FormatException>(() => Size.FromString("abc"));
            Assert.Throws<FormatException>(() => Size.FromString("123"));
            Assert.Throws<FormatException>(() => Size.FromString("123percent"));
            Assert.Throws<FormatException>(() => Size.FromString("123pixels"));
        }

        [Test]
        public void TestBitmapToImage()
        {
            string path = TestContext.CurrentContext.TestDirectory + "/TestImages/_input.bmp";
            Image img = BitmapAdapter.FromBitmapPath(path);
            int w = img.Width;
            int h = img.Height;
            Assert.AreEqual(160, w);
            Assert.AreEqual(90, h);
            Assert.AreEqual((98, 144, 198), (img[0, 0, 0], img[0, 0, 1], img[0, 0, 2]));
            Assert.AreEqual((33, 32, 27), (img[w - 1, 0, 0], img[w - 1, 0, 1], img[w - 1, 0, 2]));
            Assert.AreEqual((85, 120, 117), (img[0, h - 1, 0], img[0, h - 1, 1], img[0, h - 1, 2]));
            Assert.AreEqual((181, 174, 157), (img[w - 1, h - 1, 0], img[w - 1, h - 1, 1], img[w - 1, h - 1, 2]));
        }
    }
}