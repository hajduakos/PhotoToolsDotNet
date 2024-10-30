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
            Assert.That(f, Is.InstanceOf<AdaptiveTresholdFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "SquareSize", "8");
            AdaptiveTresholdFilter ff = f as AdaptiveTresholdFilter;
            Assert.That(ff.SquareSize, Is.EqualTo(8));
        }

        [Test]
        public void TestAdaptiveTresholdParCnt() => Assert.That(Common.ParamCount(typeof(AdaptiveTresholdFilter)), Is.EqualTo(1));

        [Test]
        public void TestOilPaint()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("OilPaint");
            Assert.That(f, Is.InstanceOf<OilPaintFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "8");
            ReflectiveApi.SetFilterPropertyByName(f, "IntensityLevels", "9");
            OilPaintFilter ff = f as OilPaintFilter;
            Assert.That(ff.Radius, Is.EqualTo(8));
            Assert.That(ff.IntensityLevels, Is.EqualTo(9));
        }

        [Test]
        public void TestOilPaintParCnt() => Assert.That(Common.ParamCount(typeof(OilPaintFilter)), Is.EqualTo(2));

        [Test]
        public void TestRandomJitter()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("RandomJitter");
            Assert.That(f, Is.InstanceOf<RandomJitterFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "8");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "1000");
            RandomJitterFilter ff = f as RandomJitterFilter;
            Assert.That(ff.Radius, Is.EqualTo(8));
            Assert.That(ff.Seed, Is.EqualTo(1000));
        }

        [Test]
        public void TestRandomJitterParCnt() => Assert.That(Common.ParamCount(typeof(RandomJitterFilter)), Is.EqualTo(2));
    }
}
