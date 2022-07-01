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
            Assert.IsInstanceOf<CrystallizeFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Size", "30");
            ReflectiveApi.SetFilterPropertyByName(f, "Averaging", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "70");
            CrystallizeFilter ff = f as CrystallizeFilter;
            Assert.AreEqual(30, ff.Size);
            Assert.AreEqual(50, ff.Averaging);
            Assert.AreEqual(70, ff.Seed);
        }

        [Test]
        public void TestCrystallizerParCnt() => Assert.AreEqual(3, Common.ParamCount(typeof(CrystallizeFilter)));

        [Test]
        public void TestLego()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Lego");
            Assert.IsInstanceOf<LegoFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Size", "30");
            LegoFilter ff = f as LegoFilter;
            Assert.AreEqual(30, ff.Size);
        }

        [Test]
        public void TestLegoParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(LegoFilter)));

        [Test]
        public void TestPixelate()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Pixelate");
            Assert.IsInstanceOf<PixelateFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Size", "30");
            ReflectiveApi.SetFilterPropertyByName(f, "Mode", "Average");
            PixelateFilter ff = f as PixelateFilter;
            Assert.AreEqual(30, ff.Size);
            Assert.AreEqual(PixelateFilter.PixelateMode.Average, ff.Mode);
        }

        [Test]
        public void TestPixelateParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(PixelateFilter)));
    }
}
