using NUnit.Framework;
using FilterLib.Util;

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
    }
}