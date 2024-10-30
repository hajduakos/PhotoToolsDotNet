using FilterLib.Filters;
using FilterLib.Filters.Mosaic;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class MosaicApiTests
    {
        [Test]
        public void TestCrystallize()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Crystallize");
            Assert.That(f, Is.InstanceOf<CrystallizeFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Size", "30");
            ReflectiveApi.SetFilterPropertyByName(f, "Averaging", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "70");
            CrystallizeFilter ff = f as CrystallizeFilter;
            Assert.That(ff.Size, Is.EqualTo(30));
            Assert.That(ff.Averaging, Is.EqualTo(50));
            Assert.That(ff.Seed, Is.EqualTo(70));
        }

        [Test]
        public void TestCrystallizerParCnt() => Assert.That(Common.ParamCount(typeof(CrystallizeFilter)), Is.EqualTo(3));

        [Test]
        public void TestLego()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Lego");
            Assert.That(f, Is.InstanceOf<LegoFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Size", "30");
            ReflectiveApi.SetFilterPropertyByName(f, "AntiAlias", "High");
            LegoFilter ff = f as LegoFilter;
            Assert.That(ff.Size, Is.EqualTo(30));
            Assert.That(ff.AntiAlias, Is.EqualTo(Util.AntiAliasQuality.High));
        }

        [Test]
        public void TestLegoParCnt() => Assert.That(Common.ParamCount(typeof(LegoFilter)), Is.EqualTo(2));

        [Test]
        public void TestPixelate()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Pixelate");
            Assert.That(f, Is.InstanceOf<PixelateFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Size", "30");
            ReflectiveApi.SetFilterPropertyByName(f, "Mode", "Average");
            PixelateFilter ff = f as PixelateFilter;
            Assert.That(ff.Size, Is.EqualTo(30));
            Assert.That(ff.Mode, Is.EqualTo(PixelateFilter.PixelateMode.Average));
        }

        [Test]
        public void TestPixelateParCnt() => Assert.That(Common.ParamCount(typeof(PixelateFilter)), Is.EqualTo(2));
    }
}
