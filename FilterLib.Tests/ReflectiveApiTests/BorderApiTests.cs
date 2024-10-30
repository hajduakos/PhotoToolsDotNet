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
            Assert.That(f, Is.InstanceOf<JitterBorderFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "8px");
            ReflectiveApi.SetFilterPropertyByName(f, "Color", "(255, 0, 0)");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "15");
            JitterBorderFilter ff = f as JitterBorderFilter;
            Assert.That(ff.Width.ToAbsolute(0), Is.EqualTo(8));
            Assert.That(ff.Color, Is.EqualTo(new RGB(255, 0, 0)));
            Assert.That(ff.Seed, Is.EqualTo(15));
        }

        [Test]
        public void TestJitterBorderParCnt() => Assert.That(Common.ParamCount(typeof(JitterBorderFilter)), Is.EqualTo(3));

        [Test]
        public void TestFadeBorder()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("FadeBorder");
            Assert.That(f, Is.InstanceOf<FadeBorderFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "8px");
            ReflectiveApi.SetFilterPropertyByName(f, "Color", "(255, 0, 0)");
            FadeBorderFilter ff = f as FadeBorderFilter;
            Assert.That(ff.Width.ToAbsolute(0), Is.EqualTo(8));
            Assert.That(ff.Color, Is.EqualTo(new RGB(255, 0, 0)));
        }

        [Test]
        public void TestFadeBorderParCnt() => Assert.That(Common.ParamCount(typeof(FadeBorderFilter)), Is.EqualTo(2));

        [Test]
        public void TestPatternBorder()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("PatternBorder");
            Assert.That(f, Is.InstanceOf<PatternBorderFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "10%");
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "8px");
            ReflectiveApi.SetFilterPropertyByName(f, "Pattern", TestContext.CurrentContext.TestDirectory + "/TestImages/_input.bmp");
            ReflectiveApi.SetFilterPropertyByName(f, "Position", "Outside");
            ReflectiveApi.SetFilterPropertyByName(f, "AntiAlias", "Low");
            PatternBorderFilter ff = f as PatternBorderFilter;
            Assert.That(ff.Width.ToAbsolute(200), Is.EqualTo(20));
            Assert.That(ff.Radius.ToAbsolute(200), Is.EqualTo(8));
            Assert.That(ff.Pattern.Width, Is.EqualTo(160));
            Assert.That(ff.Position, Is.EqualTo(BorderPosition.Outside));
            Assert.That(ff.AntiAlias, Is.EqualTo(AntiAliasQuality.Low));
        }

        [Test]
        public void TestPatternBorderParCnt() => Assert.That(Common.ParamCount(typeof(PatternBorderFilter)), Is.EqualTo(5));

        [Test]
        public void TestSimpleBorder()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("SimpleBorder");
            Assert.That(f, Is.InstanceOf<SimpleBorderFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "10%");
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "8px");
            ReflectiveApi.SetFilterPropertyByName(f, "Color", "(255, 0, 0)");
            ReflectiveApi.SetFilterPropertyByName(f, "Position", "Outside");
            ReflectiveApi.SetFilterPropertyByName(f, "AntiAlias", "High");
            SimpleBorderFilter ff = f as SimpleBorderFilter;
            Assert.That(ff.Width.ToAbsolute(200), Is.EqualTo(20));
            Assert.That(ff.Radius.ToAbsolute(200), Is.EqualTo(8));
            Assert.That(ff.Color, Is.EqualTo(new RGB(255, 0, 0)));
            Assert.That(ff.Position, Is.EqualTo(BorderPosition.Outside));
            Assert.That(ff.AntiAlias, Is.EqualTo(AntiAliasQuality.High));
        }

        [Test]
        public void TestSimpleBorderParCnt() => Assert.That(Common.ParamCount(typeof(SimpleBorderFilter)), Is.EqualTo(5));

        [Test]
        public void TestVignette()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Vignette");
            Assert.That(f, Is.InstanceOf<VignetteFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "120%");
            ReflectiveApi.SetFilterPropertyByName(f, "ClearRadius", "40px");
            ReflectiveApi.SetFilterPropertyByName(f, "Color", "(255, 127, 0)");
            VignetteFilter ff = f as VignetteFilter;
            Assert.That(ff.Radius.ToAbsolute(200), Is.EqualTo(240));
            Assert.That(ff.ClearRadius.ToAbsolute(200), Is.EqualTo(40));
            Assert.That(ff.Color, Is.EqualTo(new RGB(255, 127, 0)));
        }

        [Test]
        public void TestVignetteParCnt() => Assert.That(Common.ParamCount(typeof(VignetteFilter)), Is.EqualTo(3));
    }
}
