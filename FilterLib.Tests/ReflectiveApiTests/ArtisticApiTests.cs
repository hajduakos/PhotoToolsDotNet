using FilterLib.Filters;
using FilterLib.Filters.Artistic;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class ArtisticApiTests
    {
        [Test]
        public void TestAdaptiveTresholdParse()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("AdaptiveTreshold");
            Assert.IsInstanceOf<AdaptiveTresholdFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "SquareSize", "8");
            AdaptiveTresholdFilter ff = f as AdaptiveTresholdFilter;
            Assert.AreEqual(8, ff.SquareSize);
        }

        [Test]
        public void TestOilPaintParse()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("OilPaint");
            Assert.IsInstanceOf<OilPaintFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "8");
            OilPaintFilter ff = f as OilPaintFilter;
            Assert.AreEqual(8, ff.Radius);
        }

        [Test]
        public void TestRandomJitterParse()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("RandomJitter");
            Assert.IsInstanceOf<RandomJitterFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "8");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "1000");
            RandomJitterFilter ff = f as RandomJitterFilter;
            Assert.AreEqual(8, ff.Radius);
            Assert.AreEqual(1000, ff.Seed);
        }
    }
}
