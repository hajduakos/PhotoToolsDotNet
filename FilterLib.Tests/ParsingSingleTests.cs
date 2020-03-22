using FilterLib.Filters.Adjustments;
using FilterLib.Filters.Color;
using FilterLib.Scripting;
using NUnit.Framework;

namespace FilterLib.Tests
{
    public class ParsingSingleTests
    {

        #region Excpetions

        [Test]
        public void TestParamBeforeFilter() =>
            Assert.Throws<SyntaxException>(() => Parser.Parse(new string[] { "- Brightness: 50", "Brightness" }));

        [Test]
        public void TestNonExistingFilter() =>
            Assert.Throws<FilterNotFoundException>(() => Parser.Parse(new string[] { "NoSuchFilter" }));
        
        [Test]
        public void TestNonExistingParam() =>
            Assert.Throws<ParamNotFoundException>(() => Parser.Parse(new string[] { "Brightness", "- NoSuchParam: 0" }));
        
        [Test]
        public void TestParamInvalidSyntax() =>
            Assert.Throws<SyntaxException>(() => Parser.Parse(new string[] { "Brightness", "- Brightness" }));

        [Test]
        public void TestParamInvalidValue() =>
            Assert.Throws<ParseException>(() => Parser.Parse(new string[] { "Brightness", "- Brightness: abc" }));

        #endregion

        #region Adjustments
        [Test]
        public void TestBrightnessParse()
        {
            var filters = Parser.Parse(new string[] { "Brightness" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<BrightnessFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "Brightness", "- Brightness: 50" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<BrightnessFilter>(filters[0]);
            BrightnessFilter f = filters[0] as BrightnessFilter;
            Assert.AreEqual(50, f.Brightness);
        }

        [Test]
        public void TestColorHSLParse()
        {
            var filters = Parser.Parse(new string[] { "ColorHSL" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<ColorHSLFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "ColorHSL", "- Hue: 10", "- Saturation: 20", "- Lightness: 30" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<ColorHSLFilter>(filters[0]);
            ColorHSLFilter f = filters[0] as ColorHSLFilter;
            Assert.AreEqual(10, f.Hue);
            Assert.AreEqual(20, f.Saturation);
            Assert.AreEqual(30, f.Lightness);
        }

        [Test]
        public void TestColorRGBParse()
        {
            var filters = Parser.Parse(new string[] { "ColorRGB" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<ColorRGBFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "ColorRGB", "- Red: 10", "- Green: 20", "- Blue: 30" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<ColorRGBFilter>(filters[0]);
            ColorRGBFilter f = filters[0] as ColorRGBFilter;
            Assert.AreEqual(10, f.Red);
            Assert.AreEqual(20, f.Green);
            Assert.AreEqual(30, f.Blue);
        }

        [Test]
        public void TestContrastParse()
        {
            var filters = Parser.Parse(new string[] { "Contrast" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<ContrastFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "Contrast", "- Contrast: -50" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<ContrastFilter>(filters[0]);
            ContrastFilter f = filters[0] as ContrastFilter;
            Assert.AreEqual(-50, f.Contrast);
        }

        [Test]
        public void TestGammaParse()
        {
            var filters = Parser.Parse(new string[] { "Gamma" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<GammaFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "Gamma", "- Gamma: 1.5" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<GammaFilter>(filters[0]);
            GammaFilter f = filters[0] as GammaFilter;
            Assert.AreEqual(1.5f, f.Gamma);
        }

        [Test]
        public void TestLevelsParse()
        {
            var filters = Parser.Parse(new string[] { "Levels" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<LevelsFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "Levels", "- Dark: 10", "- Light: 240" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<LevelsFilter>(filters[0]);
            LevelsFilter f = filters[0] as LevelsFilter;
            Assert.AreEqual(10, f.Dark);
            Assert.AreEqual(240, f.Light);
        }
        #endregion

        #region Color
        [Test]
        public void TestInvertParse()
        {
            var filters = Parser.Parse(new string[] { "Invert" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<InvertFilter>(filters[0]);
        }
        #endregion
    }
}
