using NUnit.Framework;
using FilterLib.Filters.Adjustments;
using System.Collections.Generic;
using FilterLib.Filters;
using System;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class AdjustmentsTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("AutoLevels.bmp", new AutoLevelsFilter(), 1);

            yield return new TestCaseData("_input.bmp", new BrightnessFilter(0), 0);
            yield return new TestCaseData("Brightness_50.bmp", new BrightnessFilter(50), 0);
            yield return new TestCaseData("Brightness_-100.bmp", new BrightnessFilter(-100), 0);

            yield return new TestCaseData("_input.bmp", new ColorHSLFilter(0, 0, 0), 5);
            yield return new TestCaseData("ColorHSL_0_0_50.bmp", new ColorHSLFilter(0, 0, 50), 1);
            yield return new TestCaseData("ColorHSL_0_0_-50.bmp", new ColorHSLFilter(0, 0, -50), 1);
            yield return new TestCaseData("ColorHSL_0_100_0.bmp", new ColorHSLFilter(0, 100, 0), 1);
            yield return new TestCaseData("ColorHSL_0_-100_0.bmp", new ColorHSLFilter(0, -100, 0), 1);
            yield return new TestCaseData("ColorHSL_50_40_-30.bmp", new ColorHSLFilter(50, 40, -30), 1);
            yield return new TestCaseData("ColorHSL_180_0_0.bmp", new ColorHSLFilter(180, 0, 0), 1);

            yield return new TestCaseData("_input.bmp", new ColorRGBFilter(0, 0, 0), 0);
            yield return new TestCaseData("ColorRGB_0_0_100.bmp", new ColorRGBFilter(0, 0, 100), 0);
            yield return new TestCaseData("ColorRGB_0_100_0.bmp", new ColorRGBFilter(0, 100, 0), 0);
            yield return new TestCaseData("ColorRGB_100_0_0.bmp", new ColorRGBFilter(100, 0, 0), 0);
            yield return new TestCaseData("ColorRGB_50_-40_30.bmp", new ColorRGBFilter(50, -40, 30), 0);

            yield return new TestCaseData("_input.bmp", new ContrastFilter(0), 1);
            yield return new TestCaseData("Contrast_50.bmp", new ContrastFilter(50), 1);
            yield return new TestCaseData("Contrast_-50.bmp", new ContrastFilter(-50), 1);

            yield return new TestCaseData("_input.bmp", new GammaFilter(1f), 1);
            yield return new TestCaseData("Gamma_0.4.bmp", new GammaFilter(0.4f), 1);
            yield return new TestCaseData("Gamma_1.5.bmp", new GammaFilter(1.5f), 1);

            yield return new TestCaseData("_input.bmp", new LevelsFilter(0, 255), 1);
            yield return new TestCaseData("Levels_30_200.bmp", new LevelsFilter(30, 200), 1);
            yield return new TestCaseData("Levels_120_125.bmp", new LevelsFilter(120, 125), 1);

            yield return new TestCaseData("_input.bmp", new ShadowsHighlightsFilter(0, 0), 0);
            yield return new TestCaseData("ShadowsHighlights_10_40.bmp", new ShadowsHighlightsFilter(10, 40), 1);
            yield return new TestCaseData("ShadowsHighlights_100_0.bmp", new ShadowsHighlightsFilter(100, 0), 1);
            yield return new TestCaseData("ShadowsHighlights_0_100.bmp", new ShadowsHighlightsFilter(0, 100), 1);
        }

        internal static IEnumerable<TestCaseData> Exceptions()
        {
            yield return new TestCaseData("_input.bmp", new LevelsFilter(80, 80));
            yield return new TestCaseData("_input.bmp", new LevelsFilter(100, 80));
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.That(Common.CheckFilter("_input.bmp", expected, filter, tolerance));

        [Test]
        [TestCaseSource("Exceptions")]
        public void TestEx(string expected, IFilter filter) =>
            Assert.Throws<ArgumentException>(() => Common.CheckFilter("_input.bmp", expected, filter));
    }
}
