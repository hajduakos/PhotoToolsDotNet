using FilterLib.Filters;
using FilterLib.Filters.Adjustments;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class AdjustmentsApiTests
    {
        [Test]
        public void TestAutoLevels() =>
            Assert.IsInstanceOf<AutoLevelsFilter>(ReflectiveApi.ConstructFilterByName("AutoLevels"));
        
        [Test]
        public void TestBrightness()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Brightness");
            Assert.IsInstanceOf<BrightnessFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Brightness", "80");
            BrightnessFilter ff = f as BrightnessFilter;
            Assert.AreEqual(80, ff.Brightness);
        }

        [Test]
        public void TestColorHSL()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("ColorHSL");
            Assert.IsInstanceOf<ColorHSLFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Hue", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Saturation", "20");
            ReflectiveApi.SetFilterPropertyByName(f, "Lightness", "30");
            ColorHSLFilter ff = f as ColorHSLFilter;
            Assert.AreEqual(10, ff.Hue);
            Assert.AreEqual(20, ff.Saturation);
            Assert.AreEqual(30, ff.Lightness);
        }

        [Test]
        public void TestColorRGB()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("ColorRGB");
            Assert.IsInstanceOf<ColorRGBFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Red", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Green", "20");
            ReflectiveApi.SetFilterPropertyByName(f, "Blue", "30");
            ColorRGBFilter ff = f as ColorRGBFilter;
            Assert.AreEqual(10, ff.Red);
            Assert.AreEqual(20, ff.Green);
            Assert.AreEqual(30, ff.Blue);
        }

        [Test]
        public void TestContrast()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Contrast");
            Assert.IsInstanceOf<ContrastFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Contrast", "-50");
            ContrastFilter ff = f as ContrastFilter;
            Assert.AreEqual(-50, ff.Contrast);
        }

        [Test]
        public void TestGamma()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Gamma");
            Assert.IsInstanceOf<GammaFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Gamma", "1.5");
            GammaFilter ff = f as GammaFilter;
            Assert.AreEqual(1.5f, ff.Gamma);
        }

        [Test]
        public void TestLevels()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Levels");
            Assert.IsInstanceOf<LevelsFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Dark", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Light", "240");
            LevelsFilter ff = f as LevelsFilter;
            Assert.AreEqual(10, ff.Dark);
            Assert.AreEqual(240, ff.Light);
        }

        [Test]
        public void TestShadowsHighlights()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("ShadowsHighlights");
            Assert.IsInstanceOf<ShadowsHighlightsFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Brighten", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Darken", "40");
            ShadowsHighlightsFilter ff = f as ShadowsHighlightsFilter;
            Assert.AreEqual(10, ff.Brighten);
            Assert.AreEqual(40, ff.Darken);
        }
    }
}
