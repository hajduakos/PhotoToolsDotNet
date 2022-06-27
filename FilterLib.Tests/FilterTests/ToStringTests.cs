using FilterLib.Filters;
using FilterLib.Filters.Adjustments;
using FilterLib.Filters.Artistic;
using FilterLib.Filters.Blur;
using NUnit.Framework;
using System.Collections.Generic;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ToStringTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData(new AutoLevelsFilter(), "AutoLevelsFilter");
            yield return new TestCaseData(new BrightnessFilter(123), "BrightnessFilter(Brightness: 123)");
            yield return new TestCaseData(new ColorHSLFilter(12, 34, 56), "ColorHSLFilter(Hue: 12, Saturation: 34, Lightness: 56)");
            yield return new TestCaseData(new ColorRGBFilter(12, 34, 56), "ColorRGBFilter(Red: 12, Green: 34, Blue: 56)");
            yield return new TestCaseData(new ContrastFilter(123), "ContrastFilter(Contrast: 100)");
            yield return new TestCaseData(new GammaFilter(2.5f), "GammaFilter(Gamma: 2.5)");
            yield return new TestCaseData(new LevelsFilter(12, 34), "LevelsFilter(Dark: 12, Light: 34)");
            yield return new TestCaseData(new ShadowsHighlightsFilter(12, 34), "ShadowsHighlightsFilter(Brighten: 12, Darken: 34)");

            yield return new TestCaseData(new AdaptiveTresholdFilter(4), "AdaptiveTresholdFilter(SquareSize: 4)");
            yield return new TestCaseData(new OilPaintFilter(5), "OilPaintFilter(Radius: 5)");
            yield return new TestCaseData(new RandomJitterFilter(12, 34), "RandomJitterFilter(Radius: 12, Seed: 34)");

            yield return new TestCaseData(new BlurFilter(), "BlurFilter");
            yield return new TestCaseData(new BoxBlurFilter(12, 34), "BoxBlurFilter(RadiusX: 12, RadiusY: 34)");
            yield return new TestCaseData(new GaussianBlurFilter(12), "GaussianBlurFilter(Radius: 12)");
            yield return new TestCaseData(new MotionBlurFilter(12, 34), "MotionBlurFilter(Length: 12, Angle: 34)");
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(IFilter filter, string expected) =>
            Assert.AreEqual(expected, filter.ToString());
    }
}
