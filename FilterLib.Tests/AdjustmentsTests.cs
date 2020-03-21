using NUnit.Framework;
using FilterLib.Adjustments;

namespace FilterLib.Tests
{
    public class AdjustmentsTests
    {
        [Test]
        public void TestBrightness()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new BrightnessFilter(0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Brightness_50.bmp", new BrightnessFilter(50), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Brightness_-100.bmp", new BrightnessFilter(-100), 0));
        }

        [Test]
        public void TestColorRGB()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new ColorRGBFilter(0, 0, 0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ColorRGB_0_0_100.bmp", new ColorRGBFilter(0, 0, 100), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ColorRGB_0_100_0.bmp", new ColorRGBFilter(0, 100, 0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ColorRGB_100_0_0.bmp", new ColorRGBFilter(100, 0, 0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ColorRGB_50_-40_30.bmp", new ColorRGBFilter(50, -40, 30), 0));
        }

        [Test]
        public void TestContrast()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new ContrastFilter(0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Contrast_50.bmp", new ContrastFilter(50), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Contrast_-50.bmp", new ContrastFilter(-50), 1));
        }
    }
}
