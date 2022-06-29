using FilterLib.Filters;
using FilterLib.Filters.Adjustments;
using FilterLib.Filters.Artistic;
using FilterLib.Filters.Blur;
using FilterLib.Filters.Border;
using FilterLib.Util;
using NUnit.Framework;
using System.Collections.Generic;
using Bitmap = System.Drawing.Bitmap;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ToStringTests
    {
        private static readonly Bitmap pattern = new(TestContext.CurrentContext.TestDirectory + "/TestImages/_input2.bmp");
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

            yield return new TestCaseData(new FadeBorderFilter(Size.Absolute(12), new RGB(34, 56, 78)), "FadeBorderFilter(Width: 12px, Color: RGB(34, 56, 78))");
            yield return new TestCaseData(new JitterBorderFilter(Size.Absolute(12), new RGB(34, 56, 78), 9), "JitterBorderFilter(Width: 12px, Color: RGB(34, 56, 78), Seed: 9)");
            yield return new TestCaseData(new PatternBorderFilter(Size.Absolute(12), Size.Absolute(34), pattern, BorderPosition.Inside), "PatternBorderFilter(Width: 12px, Radius: 34px, Pattern: Bitmap(160x90), Position: Inside)");
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(IFilter filter, string expected) =>
            Assert.AreEqual(expected, filter.ToString());

        [OneTimeTearDown]
        public static void CleanUp() => pattern.Dispose();
    }
}
