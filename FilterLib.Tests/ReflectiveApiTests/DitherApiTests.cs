using FilterLib.Filters;
using FilterLib.Filters.Dither;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class DitherApiTests
    {
        [Test]
        public void TestBayerDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("BayerDither");
            Assert.IsInstanceOf<BayerDitherFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Size", "8");
            BayerDitherFilter ff = f as BayerDitherFilter;
            Assert.AreEqual(50, ff.Levels);
            Assert.AreEqual(8, ff.Size);
        }

        [Test]
        public void TestFloydSteinbergDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("FloydSteinbergDither");
            Assert.IsInstanceOf<FloydSteinbergDitherFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            FloydSteinbergDitherFilter ff = f as FloydSteinbergDitherFilter;
            Assert.AreEqual(50, ff.Levels);
        }

        [Test]
        public void TestJarvisJudiceNinkeDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("JarvisJudiceNinkeDither");
            Assert.IsInstanceOf<JarvisJudiceNinkeDitherFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            JarvisJudiceNinkeDitherFilter ff = f as JarvisJudiceNinkeDitherFilter;
            Assert.AreEqual(50, ff.Levels);
        }

        [Test]
        public void TestRandomDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("RandomDither");
            Assert.IsInstanceOf<RandomDitherFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            RandomDitherFilter ff = f as RandomDitherFilter;
            Assert.AreEqual(50, ff.Levels);
        }

        [Test]
        public void TestSierraDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("SierraDither");
            Assert.IsInstanceOf<SierraDitherFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            SierraDitherFilter ff = f as SierraDitherFilter;
            Assert.AreEqual(50, ff.Levels);
        }
    }
}
