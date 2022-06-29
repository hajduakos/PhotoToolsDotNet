using FilterLib.Filters;
using FilterLib.Filters.Adjustments;
using FilterLib.Filters.Artistic;
using FilterLib.Filters.Blur;
using FilterLib.Filters.Border;
using FilterLib.Filters.Color;
using FilterLib.Filters.Dither;
using FilterLib.Filters.Edges;
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
            yield return new TestCaseData(new SimpleBorderFilter(Size.Absolute(12), Size.Absolute(34), new RGB(56, 78, 90), BorderPosition.Outside), "SimpleBorderFilter(Width: 12px, Radius: 34px, Color: RGB(56, 78, 90), Position: Outside)");
            yield return new TestCaseData(new VignetteFilter(Size.Relative(1), Size.Relative(0), new RGB(12, 34, 56)), "VignetteFilter(Radius: 100%, ClearRadius: 0%, Color: RGB(12, 34, 56))");

            yield return new TestCaseData(new GradientMapFilter(new Gradient(new RGB(255, 0, 0), new RGB(255, 255, 0), new RGB(0, 255, 0))), "GradientMapFilter(GradientMap: Gradient(0 RGB(255, 0, 0), 0.5 RGB(255, 255, 0), 1 RGB(0, 255, 0)))");
            yield return new TestCaseData(new GrayscaleFilter(12, 34, 56), "GrayscaleFilter(Red: 12, Green: 34, Blue: 56)");
            yield return new TestCaseData(new InvertFilter(), "InvertFilter");
            yield return new TestCaseData(new OrtonFilter(12, 34), "OrtonFilter(Strength: 12, Radius: 34)");
            yield return new TestCaseData(new PosterizeFilter(123), "PosterizeFilter(Levels: 123)");
            yield return new TestCaseData(new SepiaFilter(), "SepiaFilter");
            yield return new TestCaseData(new TresholdFilter(123), "TresholdFilter(Treshold: 123)");
            yield return new TestCaseData(new VintageFilter(12), "VintageFilter(Strength: 12)");

            yield return new TestCaseData(new AtkinsonDitherFilter(123), "AtkinsonDitherFilter(Levels: 123)");
            yield return new TestCaseData(new BayerDitherFilter(123, 45), "BayerDitherFilter(Size: 45, Levels: 123)");
            yield return new TestCaseData(new BurkesDitherFilter(123), "BurkesDitherFilter(Levels: 123)");
            yield return new TestCaseData(new FanDitherFilter(123), "FanDitherFilter(Levels: 123)");
            yield return new TestCaseData(new FloydSteinbergDitherFilter(123), "FloydSteinbergDitherFilter(Levels: 123)");
            yield return new TestCaseData(new JarvisJudiceNinkeDitherFilter(123), "JarvisJudiceNinkeDitherFilter(Levels: 123)");
            yield return new TestCaseData(new RandomDitherFilter(12, 34), "RandomDitherFilter(Levels: 12, Seed: 34)");
            yield return new TestCaseData(new ShiauFanDitherFilter(123), "ShiauFanDitherFilter(Levels: 123)");
            yield return new TestCaseData(new SierraDitherFilter(123), "SierraDitherFilter(Levels: 123)");
            yield return new TestCaseData(new StuckiDitherFilter(123), "StuckiDitherFilter(Levels: 123)");

            yield return new TestCaseData(new EdgeDetectionFilter(), "EdgeDetectionFilter");
            yield return new TestCaseData(new EmbossFilter(), "EmbossFilter");
            yield return new TestCaseData(new PrewittFilter(), "PrewittFilter");
            yield return new TestCaseData(new SobelFilter(), "SobelFilter");
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(IFilter filter, string expected) =>
            Assert.AreEqual(expected, filter.ToString());

        [OneTimeTearDown]
        public static void CleanUp() => pattern.Dispose();
    }
}
