using FilterLib.Filters;
using FilterLib.Filters.Mosaic;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class MosaicApiTests
    {
        [Test]
        public void TestCrystallizeParse()
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
        public void TestLegoParse()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Lego");
            Assert.IsInstanceOf<LegoFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Size", "30");
            LegoFilter ff = f as LegoFilter;
            Assert.AreEqual(30, ff.Size);
        }

        [Test]
        public void TestPixelateParse()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Pixelate");
            Assert.IsInstanceOf<PixelateFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Size", "30");
            PixelateFilter ff = f as PixelateFilter;
            Assert.AreEqual(30, ff.Size);
        }
    }
}
