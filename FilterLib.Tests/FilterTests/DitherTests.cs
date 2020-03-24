using FilterLib.Filters.Dither;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class DitherTests
    {
        [Test]
        public void TestBayerDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "BayerDither_2_1.bmp", new BayerDitherFilter(2, 1), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "BayerDither_2_2.bmp", new BayerDitherFilter(2, 2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "BayerDither_2_5.bmp", new BayerDitherFilter(2, 5), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "BayerDither_4_4.bmp", new BayerDitherFilter(4, 4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new BayerDitherFilter(256, 1), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new BayerDitherFilter(256, 2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new BayerDitherFilter(256, 4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new BayerDitherFilter(128, 4), 2));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new BayerDitherFilter(64, 4), 5));
        }

        [Test]
        public void TestFloydSteinbergDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "FloydSteinbergDither_2.bmp", new FloydSteinbergDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "FloydSteinbergDither_4.bmp", new FloydSteinbergDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new FloydSteinbergDitherFilter(256), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new FloydSteinbergDitherFilter(128), 2));
        }

        [Test]
        public void TestJarvisJudiceNinkeDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "JarvisJudiceNinkeDither_2.bmp", new JarvisJudiceNinkeDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "JarvisJudiceNinkeDither_4.bmp", new JarvisJudiceNinkeDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new JarvisJudiceNinkeDitherFilter(256), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new JarvisJudiceNinkeDitherFilter(128), 2));
        }

        [Test]
        public void TestSierraDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "SierraDither_2.bmp", new SierraDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "SierraDither_4.bmp", new SierraDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new SierraDitherFilter(256), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new SierraDitherFilter(128), 2));
        }
    }
}
