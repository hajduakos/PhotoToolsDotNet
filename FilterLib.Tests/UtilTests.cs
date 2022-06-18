using NUnit.Framework;
using FilterLib.Util;
using System;

namespace FilterLib.Tests
{
    public class UtilTests
    {
        [Test]
        public void TestClamp()
        {
            Assert.AreEqual(0, (-1000).Clamp(0, 255));
            Assert.AreEqual(0, (-1).Clamp(0, 255));
            Assert.AreEqual(0, 0.Clamp(0, 255));
            Assert.AreEqual(1, 1.Clamp(0, 255));
            Assert.AreEqual(25, 25.Clamp(0, 255));
            Assert.AreEqual(127, 127.Clamp(0, 255));
            Assert.AreEqual(254, 254.Clamp(0, 255));
            Assert.AreEqual(255, 255.Clamp(0, 255));
            Assert.AreEqual(255, 256.Clamp(0, 255));
            Assert.AreEqual(255, 1000.Clamp(0, 255));
        }

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
    }
}