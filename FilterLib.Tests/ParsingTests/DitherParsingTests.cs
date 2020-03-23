using FilterLib.Filters.Dither;
using FilterLib.Scripting;
using NUnit.Framework;

namespace FilterLib.Tests.ParsingTests
{
    public class DitherParsingTests
    {
        [Test]
        public void TestFloydSteinbergDitherParse()
        {
            var filters = Parser.Parse(new string[] { "FloydSteinbergDither" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<FloydSteinbergDitherFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "FloydSteinbergDither", "- Levels: 50" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<FloydSteinbergDitherFilter>(filters[0]);
            FloydSteinbergDitherFilter f = filters[0] as FloydSteinbergDitherFilter;
            Assert.AreEqual(50, f.Levels);
        }

        [Test]
        public void TestJarvisJudiceNinkeDitherParse()
        {
            var filters = Parser.Parse(new string[] { "JarvisJudiceNinkeDither" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<JarvisJudiceNinkeDitherFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "JarvisJudiceNinkeDither", "- Levels: 50" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<JarvisJudiceNinkeDitherFilter>(filters[0]);
            JarvisJudiceNinkeDitherFilter f = filters[0] as JarvisJudiceNinkeDitherFilter;
            Assert.AreEqual(50, f.Levels);
        }

        [Test]
        public void TestSierraDitherParse()
        {
            var filters = Parser.Parse(new string[] { "SierraDither" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<SierraDitherFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "SierraDither", "- Levels: 50" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<SierraDitherFilter>(filters[0]);
            SierraDitherFilter f = filters[0] as SierraDitherFilter;
            Assert.AreEqual(50, f.Levels);
        }
    }
}
