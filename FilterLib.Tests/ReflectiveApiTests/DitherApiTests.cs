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
        public void TestBayerDitherParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(BayerDitherFilter)));

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
        public void TestFloydSteinbergDitherParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(FloydSteinbergDitherFilter)));

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
        public void TestJarvisJudiceNinkeDitherParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(JarvisJudiceNinkeDitherFilter)));

        [Test]
        public void TestRandomDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("RandomDither");
            Assert.IsInstanceOf<RandomDitherFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "123");
            RandomDitherFilter ff = f as RandomDitherFilter;
            Assert.AreEqual(50, ff.Levels);
            Assert.AreEqual(123, ff.Seed);
        }

        [Test]
        public void TestRandomDitherParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(RandomDitherFilter)));

        [Test]
        public void TestSierraDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("SierraDither");
            Assert.IsInstanceOf<SierraDitherFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            SierraDitherFilter ff = f as SierraDitherFilter;
            Assert.AreEqual(50, ff.Levels);
        }

        [Test]
        public void TestSierraDitherParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(SierraDitherFilter)));
    }
}
