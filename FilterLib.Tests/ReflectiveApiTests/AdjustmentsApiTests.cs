using FilterLib.Filters;
using FilterLib.Filters.Adjustments;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class AdjustmentsApiTests
    {
        [Test]
        public void TestAutoLevels() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("AutoLevels"), Is.InstanceOf<AutoLevelsFilter>());

        [Test]
        public void TestAutoLevelsParCnt() => Assert.That(Common.ParamCount(typeof(AutoLevelsFilter)), Is.EqualTo(0));
        
        [Test]
        public void TestBrightness()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Brightness");
            Assert.That(f, Is.InstanceOf<BrightnessFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Brightness", "80");
            BrightnessFilter ff = f as BrightnessFilter;
            Assert.That(ff.Brightness, Is.EqualTo(80));
        }

        [Test]
        public void TestBrightnessParCnt() => Assert.That(Common.ParamCount(typeof(BrightnessFilter)), Is.EqualTo(1));

        [Test]
        public void TestColorHSL()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("ColorHSL");
            Assert.That(f, Is.InstanceOf<ColorHSLFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Hue", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Saturation", "20");
            ReflectiveApi.SetFilterPropertyByName(f, "Lightness", "30");
            ColorHSLFilter ff = f as ColorHSLFilter;
            Assert.That(ff.Hue, Is.EqualTo(10));
            Assert.That(ff.Saturation, Is.EqualTo(20));
            Assert.That(ff.Lightness, Is.EqualTo(30));
        }

        [Test]
        public void TestColorHSLParCnt() => Assert.That(Common.ParamCount(typeof(ColorHSLFilter)), Is.EqualTo(3));

        [Test]
        public void TestColorRGB()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("ColorRGB");
            Assert.That(f, Is.InstanceOf<ColorRGBFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Red", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Green", "20");
            ReflectiveApi.SetFilterPropertyByName(f, "Blue", "30");
            ColorRGBFilter ff = f as ColorRGBFilter;
            Assert.That(ff.Red, Is.EqualTo(10));
            Assert.That(ff.Green, Is.EqualTo(20));
            Assert.That(ff.Blue, Is.EqualTo(30));
        }

        [Test]
        public void TestColorRGBParCnt() => Assert.That(Common.ParamCount(typeof(ColorRGBFilter)), Is.EqualTo(3));

        [Test]
        public void TestContrast()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Contrast");
            Assert.That(f, Is.InstanceOf<ContrastFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Contrast", "-50");
            ContrastFilter ff = f as ContrastFilter;
            Assert.That(ff.Contrast, Is.EqualTo(-50));
        }

        [Test]
        public void TestContrastParCnt() => Assert.That(Common.ParamCount(typeof(ContrastFilter)), Is.EqualTo(1));

        [Test]
        public void TestGamma()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Gamma");
            Assert.That(f, Is.InstanceOf<GammaFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Gamma", "1.5");
            GammaFilter ff = f as GammaFilter;
            Assert.That(ff.Gamma, Is.EqualTo(1.5f));
        }

        [Test]
        public void TestGammaParCnt() => Assert.That(Common.ParamCount(typeof(GammaFilter)), Is.EqualTo(1));

        [Test]
        public void TestLevels()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Levels");
            Assert.That(f, Is.InstanceOf<LevelsFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Dark", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Light", "240");
            LevelsFilter ff = f as LevelsFilter;
            Assert.That(ff.Dark, Is.EqualTo(10));
            Assert.That(ff.Light, Is.EqualTo(240));
        }

        [Test]
        public void TestLevelsParCnt() => Assert.That(Common.ParamCount(typeof(LevelsFilter)), Is.EqualTo(2));

        [Test]
        public void TestShadowsHighlights()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("ShadowsHighlights");
            Assert.That(f, Is.InstanceOf<ShadowsHighlightsFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Brighten", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Darken", "40");
            ShadowsHighlightsFilter ff = f as ShadowsHighlightsFilter;
            Assert.That(ff.Brighten, Is.EqualTo(10));
            Assert.That(ff.Darken, Is.EqualTo(40));
        }

        [Test]
        public void TestShadowsHighlightsParCnt() => Assert.That(Common.ParamCount(typeof(ShadowsHighlightsFilter)), Is.EqualTo(2));
    }
}
