using FilterLib.Filters.Dither;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class DitherTests
    {
        [Test]
        public void TestFloydSteinbergDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "FloydSteinbergDither_2.bmp", new FloydSteinbergDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "FloydSteinbergDither_4.bmp", new FloydSteinbergDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new FloydSteinbergDitherFilter(256), 1));
        }

        [Test]
        public void TestJarvisJudiceNinkeDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "JarvisJudiceNinkeDither_2.bmp", new JarvisJudiceNinkeDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "JarvisJudiceNinkeDither_4.bmp", new JarvisJudiceNinkeDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new JarvisJudiceNinkeDitherFilter(256), 1));
        }

        [Test]
        public void TestSierraDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "SierraDither_2.bmp", new SierraDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "SierraDither_4.bmp", new SierraDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new SierraDitherFilter(256), 1));
        }
    }
}
