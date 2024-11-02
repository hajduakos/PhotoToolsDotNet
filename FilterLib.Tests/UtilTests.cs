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
        public void TestClamp(int expected, int actual) => Assert.That(actual.Clamp(0, 255), Is.EqualTo(expected));

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
        public void TestClampToByteInt(byte expected, int actual) => Assert.That(actual.ClampToByte(), Is.EqualTo(expected));

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
        public void TestClampToByteFloat(byte expected, float actual) => Assert.That(actual.ClampToByte(), Is.EqualTo(expected));

        [Test]
        public void TestRGB()
        {
            Assert.That(new RGB(20, 10, 50).R, Is.EqualTo(20));
            Assert.That(new RGB(20, 10, 50).G, Is.EqualTo(10));
            Assert.That(new RGB(20, 10, 50).B, Is.EqualTo(50));
            Assert.That(new RGB(-10, 270, 1000), Is.EqualTo(new RGB(0, 255, 255)));
            Assert.That(new RGB(0, 255, 255).Equals(new RGB(-10, 270, 1000)));
            Assert.That(new RGB(0, 255, 255) == new RGB(-10, 270, 1000));
            Assert.That(new RGB(0, 255, 255) != new RGB(1, 2, 3));
        }

        [Test]
        public void TestHSL()
        {
            Assert.That(new HSL(20, 10, 50).H, Is.EqualTo(20));
            Assert.That(new HSL(20, 10, 50).S, Is.EqualTo(10));
            Assert.That(new HSL(20, 10, 50).L, Is.EqualTo(50));
            Assert.That(new HSL(370, 200, -10), Is.EqualTo(new HSL(10, 100, 0)));
            Assert.That(new HSL(10, 100, 0).Equals(new HSL(370, 200, -10)));
            Assert.That(new HSL(10, 100, 0) == new HSL(370, 200, -10));
            Assert.That(new HSL(10, 100, 0) != new HSL(1, 2, 3));
        }

        [Test]
        public void TestRGBtoHSL()
        {
            Assert.That(new RGB(0, 0, 0).ToHSL(), Is.EqualTo(new HSL(0, 0, 0)));
            Assert.That(new RGB(255, 0, 0).ToHSL(), Is.EqualTo(new HSL(0, 100, 50)));
            Assert.That(new RGB(0, 255, 0).ToHSL(), Is.EqualTo(new HSL(120, 100, 50)));
            Assert.That(new RGB(0, 0, 255).ToHSL(), Is.EqualTo(new HSL(240, 100, 50)));
            Assert.That(new RGB(30, 40, 50).ToHSL(), Is.EqualTo(new HSL(210, 25, 15)));
            Assert.That(new RGB(255, 255, 255).ToHSL(), Is.EqualTo(new HSL(0, 0, 100)));
        }

        [Test]
        public void TestHSLtoRGB()
        {
            Assert.That(new HSL(0, 0, 0).ToRGB(), Is.EqualTo(new RGB(0, 0, 0)));
            Assert.That(new HSL(0, 100, 50).ToRGB(), Is.EqualTo(new RGB(255, 0, 0)));
            Assert.That(new HSL(120, 100, 50).ToRGB(), Is.EqualTo(new RGB(0, 255, 0)));
            Assert.That(new HSL(240, 100, 50).ToRGB(), Is.EqualTo(new RGB(0, 0, 255)));
            Assert.That(new HSL(210, 25, 15).ToRGB(), Is.EqualTo(new RGB(28, 38, 47)));
            Assert.That(new HSL(0, 0, 100).ToRGB(), Is.EqualTo(new RGB(255, 255, 255)));
        }

        [Test]
        public void TestConvolutionMatrixParse()
        {
            ConvolutionMatrix c = new("[[1, 2, 3], [4, 5, 6], [7, 8, 9]] / 10 + 11");
            Assert.That(c.Width, Is.EqualTo(3));
            Assert.That(c.Height, Is.EqualTo(3));
            Assert.That(c[0, 0], Is.EqualTo(1));
            Assert.That(c[0, 1], Is.EqualTo(2));
            Assert.That(c[0, 2], Is.EqualTo(3));
            Assert.That(c[1, 0], Is.EqualTo(4));
            Assert.That(c[1, 1], Is.EqualTo(5));
            Assert.That(c[1, 2], Is.EqualTo(6));
            Assert.That(c[2, 0], Is.EqualTo(7));
            Assert.That(c[2, 1], Is.EqualTo(8));
            Assert.That(c[2, 2], Is.EqualTo(9));
            Assert.That(c.Divisor, Is.EqualTo(10));
            Assert.That(c.Bias, Is.EqualTo(11));
        }

        [Test]
        public void TestGradientOf2()
        {
            Gradient g = new(new RGB(255, 100, 0), new RGB(0, 50, 255));
            Assert.That(g.GetColor(0f), Is.EqualTo(new RGB(255, 100, 0)));
            Assert.That(g.GetColor(0.5f), Is.EqualTo(new RGB(127, 75, 127)));
            Assert.That(g.GetColor(0.8f), Is.EqualTo(new RGB(51, 60, 204)));
            Assert.That(g.GetColor(1f), Is.EqualTo(new RGB(0, 50, 255)));
        }

        [Test]
        public void TestGradientOf3()
        {
            Gradient g = new(new RGB(255, 0, 0), new RGB(255, 255, 0), new RGB(0, 255, 0));
            Assert.That(g.GetColor(0f), Is.EqualTo(new RGB(255, 0, 0)));
            Assert.That(g.GetColor(0.25f), Is.EqualTo(new RGB(255, 127, 0)));
            Assert.That(g.GetColor(0.5f), Is.EqualTo(new RGB(255, 255, 0)));
            Assert.That(g.GetColor(0.75f), Is.EqualTo(new RGB(127, 255, 0)));
            Assert.That(g.GetColor(1f), Is.EqualTo(new RGB(0, 255, 0)));
        }

        [Test]
        public void TestGradientParseOf2()
        {
            Gradient g = new("0 (255 100 0), 1 (0 50 255)");
            Assert.That(g.GetColor(0f), Is.EqualTo(new RGB(255, 100, 0)));
            Assert.That(g.GetColor(0.5f), Is.EqualTo(new RGB(127, 75, 127)));
            Assert.That(g.GetColor(0.8f), Is.EqualTo(new RGB(51, 60, 204)));
            Assert.That(g.GetColor(1f), Is.EqualTo(new RGB(0, 50, 255)));
        }

        [Test]
        public void TestGradientParseOf3()
        {
            Gradient g = new("0 (255 0 0), 0.5 (255 255 0), 1 (0 255 0)");
            Assert.That(g.GetColor(0f), Is.EqualTo(new RGB(255, 0, 0)));
            Assert.That(g.GetColor(0.25f), Is.EqualTo(new RGB(255, 127, 0)));
            Assert.That(g.GetColor(0.5f), Is.EqualTo(new RGB(255, 255, 0)));
            Assert.That(g.GetColor(0.75f), Is.EqualTo(new RGB(127, 255, 0)));
            Assert.That(g.GetColor(1f), Is.EqualTo(new RGB(0, 255, 0)));
        }

        [Test]
        public void TestGradientParseExceptions()
        {
            Assert.Throws<FormatException>(() => new Gradient("0 (0 0 0)"));
            Assert.Throws<FormatException>(() => new Gradient("a , b"));
            Assert.Throws<ArgumentException>(() => new Gradient("0 (0 0 0), 0 (0 0 0)"));
        }

        [Test]
        public void TestGradientExceptions()
        {
            Assert.Throws<ArgumentException>(() => new Gradient(new RGB[] { new(), new() }, new[] { 0f, .5f, 1f }));
            Assert.Throws<ArgumentException>(() => new Gradient(new RGB[] { new() }, new[] { 1f }));
        }

        [Test]
        public void TestSize()
        {
            Assert.That(Size.Absolute(25).ToAbsolute(0), Is.EqualTo(25));
            Assert.That(Size.Relative(.25f).ToAbsolute(0), Is.EqualTo(0));
            Assert.That(Size.Relative(.25f).ToAbsolute(100), Is.EqualTo(25));
            Assert.That(Size.Relative(2f).ToAbsolute(100), Is.EqualTo(200));
        }

        [Test]
        public void TestSizeParse()
        {
            Assert.That(Size.FromString("25px").ToAbsolute(0), Is.EqualTo(25));
            Assert.That(Size.FromString("25%").ToAbsolute(0), Is.EqualTo(0));
            Assert.That(Size.FromString("25 %").ToAbsolute(100), Is.EqualTo(25));
            Assert.That(Size.FromString("200   %").ToAbsolute(100), Is.EqualTo(200));

            Assert.Throws<FormatException>(() => Size.FromString("abc"));
            Assert.Throws<FormatException>(() => Size.FromString("123"));
            Assert.Throws<FormatException>(() => Size.FromString("123percent"));
            Assert.Throws<FormatException>(() => Size.FromString("123pixels"));
        }
    }
}