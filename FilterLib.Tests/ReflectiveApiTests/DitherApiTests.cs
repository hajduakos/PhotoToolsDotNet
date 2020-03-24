using FilterLib.Filters;
using FilterLib.Filters.Dither;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class DitherApiTests
    {
        [Test]
        public void TestBayerDitherParse()
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
        public void TestFloydSteinbergDitherParse()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("FloydSteinbergDither");
            Assert.IsInstanceOf<FloydSteinbergDitherFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            FloydSteinbergDitherFilter ff = f as FloydSteinbergDitherFilter;
            Assert.AreEqual(50, ff.Levels);
        }

        [Test]
        public void TestJarvisJudiceNinkeDitherParse()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("JarvisJudiceNinkeDither");
            Assert.IsInstanceOf<JarvisJudiceNinkeDitherFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            JarvisJudiceNinkeDitherFilter ff = f as JarvisJudiceNinkeDitherFilter;
            Assert.AreEqual(50, ff.Levels);
        }

        [Test]
        public void TestSierraDitherParse()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("SierraDither");
            Assert.IsInstanceOf<SierraDitherFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            SierraDitherFilter ff = f as SierraDitherFilter;
            Assert.AreEqual(50, ff.Levels);
        }
    }
}
