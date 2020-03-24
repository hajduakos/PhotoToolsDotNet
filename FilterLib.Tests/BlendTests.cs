using FilterLib.Blending;
using NUnit.Framework;

namespace FilterLib.Tests
{
    public class BlendTests
    {

        [Test]
        public void TestColorDodgeBlend()
        {
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "_input.bmp", new ColorDodgeBlend(0), 2));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "ColorDodgeBlend_80.bmp", new ColorDodgeBlend(80), 2));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "ColorDodgeBlend_100.bmp", new ColorDodgeBlend(100), 2));
        }

        [Test]
        public void TestDarkenBlend()
        {
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "_input.bmp", new DarkenBlend(0), 2));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "DarkenBlend_80.bmp", new DarkenBlend(80), 2));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "DarkenBlend_100.bmp", new DarkenBlend(100), 2));
        }

        [Test]
        public void TestLightenBlend()
        {
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "_input.bmp", new LightenBlend(0), 2));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "LightenBlend_80.bmp", new LightenBlend(80), 2));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "LightenBlend_100.bmp", new LightenBlend(100), 2));
        }

        [Test]
        public void TestMultiplyBlend()
        {
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "_input.bmp", new MultiplyBlend(0), 2));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "MultiplyBlend_80.bmp", new MultiplyBlend(80), 2));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "MultiplyBlend_100.bmp", new MultiplyBlend(100), 2));
        }

        [Test]
        public void TestNormalBlend()
        {
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "_input.bmp", new NormalBlend(0), 1));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "NormalBlend_50.bmp", new NormalBlend(50), 1));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "_input2.bmp", new NormalBlend(100), 1));
        }

        [Test]
        public void TestScreenBlend()
        {
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "_input.bmp", new ScreenBlend(0), 2));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "ScreenBlend_80.bmp", new ScreenBlend(80), 2));
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", "ScreenBlend_100.bmp", new ScreenBlend(100), 2));
        }
    }
}
