using FilterLib.Filters;
using FilterLib.Filters.Border;
using FilterLib.Util;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class BorderApiTests
    {
        [Test]
        public void TestJitterBorder()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("JitterBorder");
            Assert.IsInstanceOf<JitterBorderFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "8px");
            ReflectiveApi.SetFilterPropertyByName(f, "Color", "(255, 0, 0)");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "15");
            JitterBorderFilter ff = f as JitterBorderFilter;
            Assert.AreEqual(8, ff.Width.ToAbsolute(0));
            Assert.AreEqual(new RGB(255, 0, 0), ff.Color);
            Assert.AreEqual(15, ff.Seed);
        }

        [Test]
        public void TestJitterBorderParCnt() => Assert.AreEqual(3, Common.ParamCount(typeof(JitterBorderFilter)));

        [Test]
        public void TestFadeBorder()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("FadeBorder");
            Assert.IsInstanceOf<FadeBorderFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "8px");
            ReflectiveApi.SetFilterPropertyByName(f, "Color", "(255, 0, 0)");
            FadeBorderFilter ff = f as FadeBorderFilter;
            Assert.AreEqual(8, ff.Width.ToAbsolute(0));
            Assert.AreEqual(new RGB(255, 0, 0), ff.Color);
        }

        [Test]
        public void TestFadeBorderParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(FadeBorderFilter)));

        [Test]
        public void TestPatternBorder()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("PatternBorder");
            Assert.IsInstanceOf<PatternBorderFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "10%");
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "8px");
            ReflectiveApi.SetFilterPropertyByName(f, "Pattern", TestContext.CurrentContext.TestDirectory + "/TestImages/_input.bmp");
            ReflectiveApi.SetFilterPropertyByName(f, "Position", "Outside");
            PatternBorderFilter ff = f as PatternBorderFilter;
            Assert.AreEqual(20, ff.Width.ToAbsolute(200));
            Assert.AreEqual(8, ff.Radius.ToAbsolute(200));
            Assert.AreEqual(160, ff.Pattern.Width);
            Assert.AreEqual(BorderPosition.Outside, ff.Position);
        }

        [Test]
        public void TestPatternBorderParCnt() => Assert.AreEqual(4, Common.ParamCount(typeof(PatternBorderFilter)));

        [Test]
        public void TestSimpleBorder()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("SimpleBorder");
            Assert.IsInstanceOf<SimpleBorderFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "10%");
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "8px");
            ReflectiveApi.SetFilterPropertyByName(f, "Color", "(255, 0, 0)");
            ReflectiveApi.SetFilterPropertyByName(f, "Position", "Outside");
            SimpleBorderFilter ff = f as SimpleBorderFilter;
            Assert.AreEqual(20, ff.Width.ToAbsolute(200));
            Assert.AreEqual(8, ff.Radius.ToAbsolute(200));
            Assert.AreEqual(new RGB(255, 0, 0), ff.Color);
            Assert.AreEqual(BorderPosition.Outside, ff.Position);
        }

        [Test]
        public void TestSimpleBorderParCnt() => Assert.AreEqual(4, Common.ParamCount(typeof(SimpleBorderFilter)));

        [Test]
        public void TestVignette()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Vignette");
            Assert.IsInstanceOf<VignetteFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "120%");
            ReflectiveApi.SetFilterPropertyByName(f, "ClearRadius", "40px");
            ReflectiveApi.SetFilterPropertyByName(f, "Color", "(255, 127, 0)");
            VignetteFilter ff = f as VignetteFilter;
            Assert.AreEqual(240, ff.Radius.ToAbsolute(200));
            Assert.AreEqual(40, ff.ClearRadius.ToAbsolute(200));
            Assert.AreEqual(new RGB(255, 127, 0), ff.Color);
        }

        [Test]
        public void TestVignetteParCnt() => Assert.AreEqual(3, Common.ParamCount(typeof(VignetteFilter)));
    }
}
