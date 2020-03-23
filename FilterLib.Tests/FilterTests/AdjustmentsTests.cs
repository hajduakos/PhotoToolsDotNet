using NUnit.Framework;
using FilterLib.Filters.Adjustments;

namespace FilterLib.Tests.FilterTests
{
    public class AdjustmentsTests
    {
        [Test]
        public void TestAutoLevels() =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "AutoLevels.bmp", new AutoLevelsFilter(), 1));

        [Test]
        public void TestBrightness()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new BrightnessFilter(0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Brightness_50.bmp", new BrightnessFilter(50), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Brightness_-100.bmp", new BrightnessFilter(-100), 0));
        }

        [Test]
        public void TestColorHSL()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new ColorHSLFilter(0, 0, 0), 5));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ColorHSL_0_0_50.bmp", new ColorHSLFilter(0, 0, 50), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ColorHSL_0_0_-50.bmp", new ColorHSLFilter(0, 0, -50), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ColorHSL_0_100_0.bmp", new ColorHSLFilter(0, 100, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ColorHSL_0_-100_0.bmp", new ColorHSLFilter(0, -100, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ColorHSL_50_40_-30.bmp", new ColorHSLFilter(50, 40, -30), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ColorHSL_180_0_0.bmp", new ColorHSLFilter(180, 0, 0), 1));
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

        [Test]
        public void TestGamma()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new GammaFilter(1f), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Gamma_0.4.bmp", new GammaFilter(0.4f), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Gamma_1.5.bmp", new GammaFilter(1.5f), 1));
        }

        [Test]
        public void TestLevels()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new LevelsFilter(0, 255), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Levels_30_200.bmp", new LevelsFilter(30, 200), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Levels_120_125.bmp", new LevelsFilter(120, 125), 1));
        }

        [Test]
        public void TestShadowsHighlights()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new ShadowsHighlightsFilter(0, 0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ShadowsHighlights_10_40.bmp", new ShadowsHighlightsFilter(10, 40), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ShadowsHighlights_100_0.bmp", new ShadowsHighlightsFilter(100, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ShadowsHighlights_0_100.bmp", new ShadowsHighlightsFilter(0, 100), 1));
        }
    }
}
