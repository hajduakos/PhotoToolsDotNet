using FilterLib.Blending;
using FilterLib.Blending.Cancelation;
using FilterLib.Blending.Component;
using FilterLib.Blending.Contrast;
using FilterLib.Blending.Darken;
using FilterLib.Blending.Inversion;
using FilterLib.Blending.Lighten;
using FilterLib.Blending.Normal;
using FilterLib.Filters;
using FilterLib.Filters.Adjustments;
using FilterLib.Filters.Artistic;
using FilterLib.Filters.Blur;
using FilterLib.Filters.Border;
using FilterLib.Filters.Color;
using FilterLib.Filters.Dither;
using FilterLib.Filters.Edges;
using FilterLib.Filters.Generate;
using FilterLib.Filters.Mosaic;
using FilterLib.Filters.Noise;
using FilterLib.Filters.Other;
using FilterLib.Filters.Sharpen;
using FilterLib.Filters.Transform;
using FilterLib.Reporting;
using FilterLib.Util;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FilterLib.Tests
{
    [TestFixture]
    public class ReportingTests
    {
        internal static IEnumerable<TestCaseData> FilterTestCases()
        {
            yield return new TestCaseData(new AutoLevelsFilter());
            yield return new TestCaseData(new BrightnessFilter());
            yield return new TestCaseData(new ColorHSLFilter());
            yield return new TestCaseData(new ColorRGBFilter());
            yield return new TestCaseData(new ContrastFilter());
            yield return new TestCaseData(new GammaFilter());
            yield return new TestCaseData(new LevelsFilter());
            yield return new TestCaseData(new ShadowsHighlightsFilter());

            yield return new TestCaseData(new AdaptiveTresholdFilter());
            yield return new TestCaseData(new OilPaintFilter());
            yield return new TestCaseData(new RandomJitterFilter());

            yield return new TestCaseData(new BlurFilter());
            yield return new TestCaseData(new BoxBlurFilter());
            yield return new TestCaseData(new GaussianBlurFilter(1));
            yield return new TestCaseData(new MotionBlurFilter());

            yield return new TestCaseData(new FadeBorderFilter(Size.Absolute(1), new RGB()));
            yield return new TestCaseData(new JitterBorderFilter());
            yield return new TestCaseData(new PatternBorderFilter());
            yield return new TestCaseData(new SimpleBorderFilter());
            yield return new TestCaseData(new VignetteFilter());

            yield return new TestCaseData(new GradientMapFilter());
            yield return new TestCaseData(new GrayscaleFilter());
            yield return new TestCaseData(new InvertFilter());
            yield return new TestCaseData(new OctreeQuantizerFilter());
            yield return new TestCaseData(new OrtonFilter());
            yield return new TestCaseData(new PosterizeFilter());
            yield return new TestCaseData(new SepiaFilter());
            yield return new TestCaseData(new SolarizeFilter());
            yield return new TestCaseData(new TresholdFilter());
            yield return new TestCaseData(new VintageFilter());

            yield return new TestCaseData(new AtkinsonDitherFilter());
            yield return new TestCaseData(new BayerDitherFilter());
            yield return new TestCaseData(new BurkesDitherFilter());
            yield return new TestCaseData(new FanDitherFilter());
            yield return new TestCaseData(new FloydSteinbergDitherFilter());
            yield return new TestCaseData(new JarvisJudiceNinkeDitherFilter());
            yield return new TestCaseData(new RandomDitherFilter());
            yield return new TestCaseData(new ShiauFanDitherFilter());
            yield return new TestCaseData(new SierraDitherFilter());
            yield return new TestCaseData(new StuckiDitherFilter());

            yield return new TestCaseData(new EdgeDetectionFilter());
            yield return new TestCaseData(new EmbossFilter());
            yield return new TestCaseData(new PrewittFilter());
            yield return new TestCaseData(new SobelFilter());

            yield return new TestCaseData(new LinearGradientFilter());
            yield return new TestCaseData(new MarbleFilter());
            yield return new TestCaseData(new RadialGradientFilter());
            yield return new TestCaseData(new TurbulenceFilter());
            yield return new TestCaseData(new WoodRingsFilter());

            yield return new TestCaseData(new CrystallizeFilter());
            yield return new TestCaseData(new LegoFilter());
            yield return new TestCaseData(new PixelateFilter());

            yield return new TestCaseData(new AddNoiseFilter());
            yield return new TestCaseData(new MedianFilter());

            yield return new TestCaseData(new ConvertToPolarFilter());
            yield return new TestCaseData(new ConvolutionFilter(new Conv3x3(0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0)));
            yield return new TestCaseData(new EquirectangularToStereographicFilter());
            yield return new TestCaseData(new WavesFilter());

            yield return new TestCaseData(new MeanRemovalFilter());
            yield return new TestCaseData(new SharpenFilter());

            yield return new TestCaseData(new BoxDownscaleFilter());
            yield return new TestCaseData(new CropFilter());
            yield return new TestCaseData(new FlipHorizontalFilter());
            yield return new TestCaseData(new FlipVerticalFilter());
            yield return new TestCaseData(new ResizeFilter());
            yield return new TestCaseData(new Rotate180Filter());
            yield return new TestCaseData(new RotateFilter(10));
            yield return new TestCaseData(new RotateLeftFilter());
            yield return new TestCaseData(new RotateRightFilter());
        }
        internal static IEnumerable<TestCaseData> BlendTestCases()
        {
            yield return new TestCaseData(new DivideBlend());
            yield return new TestCaseData(new SubtractBlend());

            yield return new TestCaseData(new ColorBlend());
            yield return new TestCaseData(new HueBlend());
            yield return new TestCaseData(new LightnessBlend());
            yield return new TestCaseData(new SaturationBlend());

            yield return new TestCaseData(new HardLightBlend());
            yield return new TestCaseData(new HardMixBlend());
            yield return new TestCaseData(new LinearLightBlend());
            yield return new TestCaseData(new OverlayBlend());
            yield return new TestCaseData(new PinLightBlend());
            yield return new TestCaseData(new SoftLightBlend());
            yield return new TestCaseData(new VividLightBlend());

            yield return new TestCaseData(new ColorBurnBlend());
            yield return new TestCaseData(new DarkenBlend());
            yield return new TestCaseData(new DarkerColorBlend());
            yield return new TestCaseData(new LinearBurnBlend());
            yield return new TestCaseData(new MultiplyBlend());

            yield return new TestCaseData(new DifferenceBlend());
            yield return new TestCaseData(new ExcludeBlend());

            yield return new TestCaseData(new ColorDodgeBlend());
            yield return new TestCaseData(new LightenBlend());
            yield return new TestCaseData(new LighterColorBlend());
            yield return new TestCaseData(new LinearDodgeBlend());
            yield return new TestCaseData(new ScreenBlend());

            yield return new TestCaseData(new NormalBlend());
        }

        private sealed class ReporterStub : IReporter
        {

            public bool StartCalled { get; private set; }
            public bool Reported { get; private set; }
            public bool DoneCalled { get; private set; }

            public ReporterStub()
            {
                StartCalled = false;
                DoneCalled = false;
                Reported = false;
            }

            public void Done() => DoneCalled = true;

            public void Report(int value, int min = 0, int max = 100) => Reported = true;

            public void Start() => StartCalled = true;
        }

        private readonly Image img = new(10, 10);

        [Test]
        [TestCaseSource("FilterTestCases")]
        public void TestFilters(IFilter filter)
        {
            ReporterStub rep = new();
            filter.Apply(img, rep);
            Assert.IsTrue(rep.StartCalled);
            Assert.IsTrue(rep.Reported);
            Assert.IsTrue(rep.DoneCalled);
        }

        [Test]
        [TestCaseSource("BlendTestCases")]
        public void TestBlends(IBlend blend)
        {
            ReporterStub rep = new();
            blend.Apply(img, img, rep);
            Assert.IsTrue(rep.StartCalled);
            Assert.IsTrue(rep.Reported);
            Assert.IsTrue(rep.DoneCalled);
        }

        [Test]
        public void TestAllFilters()
        {
            foreach (var filter in ReflectiveApi.GetFilterTypes())
            {
                bool found = FilterTestCases().Any(tc => tc.Arguments[0].GetType() == filter);
                Assert.IsTrue(found, $"Reporting test not found for {filter.Name}");
            }
        }

        [Test]
        public void TestAllBlends()
        {
            foreach (var blend in ReflectiveApi.GetBlendTypes())
            {
                bool found = BlendTestCases().Any(tc => tc.Arguments[0].GetType() == blend);
                Assert.IsTrue(found, $"Reporting test not found for {blend.Name}");
            }
        }
    }
}
