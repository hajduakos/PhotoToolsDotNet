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
    }
}
