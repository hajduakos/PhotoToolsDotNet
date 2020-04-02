using FilterLib.Filters;
using FilterLib.Filters.Border;
using FilterLib.Util;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class BorderTests
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
        public void TestVignette()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Vignette");
            Assert.IsInstanceOf<VignetteFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "120%");
            ReflectiveApi.SetFilterPropertyByName(f, "ClearRadius", "40px");
            VignetteFilter ff = f as VignetteFilter;
            Assert.AreEqual(240, ff.Radius.ToAbsolute(200));
            Assert.AreEqual(40, ff.ClearRadius.ToAbsolute(200));
        }

        [Test]
        public void TestVignetteParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(VignetteFilter)));
    }
}
