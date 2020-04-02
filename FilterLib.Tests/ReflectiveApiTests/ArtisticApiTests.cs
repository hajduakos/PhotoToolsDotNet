using FilterLib.Filters;
using FilterLib.Filters.Artistic;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class ArtisticApiTests
    {
        [Test]
        public void TestAdaptiveTreshold()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("AdaptiveTreshold");
            Assert.IsInstanceOf<AdaptiveTresholdFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "SquareSize", "8");
            AdaptiveTresholdFilter ff = f as AdaptiveTresholdFilter;
            Assert.AreEqual(8, ff.SquareSize);
        }

        [Test]
        public void TestAdaptiveTresholdParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(AdaptiveTresholdFilter)));

        [Test]
        public void TestOilPaint()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("OilPaint");
            Assert.IsInstanceOf<OilPaintFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "8");
            OilPaintFilter ff = f as OilPaintFilter;
            Assert.AreEqual(8, ff.Radius);
        }

        [Test]
        public void TestOilPaintParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(OilPaintFilter)));

        [Test]
        public void TestRandomJitter()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("RandomJitter");
            Assert.IsInstanceOf<RandomJitterFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "8");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "1000");
            RandomJitterFilter ff = f as RandomJitterFilter;
            Assert.AreEqual(8, ff.Radius);
            Assert.AreEqual(1000, ff.Seed);
        }

        [Test]
        public void TestRandomJitterParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(RandomJitterFilter)));
    }
}
