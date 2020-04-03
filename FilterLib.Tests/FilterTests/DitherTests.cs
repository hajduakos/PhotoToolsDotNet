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
        public void TestAtkinsonDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "AtkinsonDither_2.bmp", new AtkinsonDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "AtkinsonDither_4.bmp", new AtkinsonDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new AtkinsonDitherFilter(256), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new AtkinsonDitherFilter(128), 2));
        }

        [Test]
        public void TestFanDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "FanDither_2.bmp", new FanDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "FanDither_4.bmp", new FanDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new FanDitherFilter(256), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new FanDitherFilter(128), 2));
        }

        [Test]
        public void TestShiauFanDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ShiauFanDither_2.bmp", new ShiauFanDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "ShiauFanDither_4.bmp", new ShiauFanDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new ShiauFanDitherFilter(256), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new ShiauFanDitherFilter(128), 2));
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
        public void TestRandomDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "RandomDither_2.bmp", new RandomDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "RandomDither_4.bmp", new RandomDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new FloydSteinbergDitherFilter(256), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new FloydSteinbergDitherFilter(128), 2));
        }

        [Test]
        public void TestSierraDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "SierraDither_2.bmp", new SierraDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "SierraDither_4.bmp", new SierraDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new SierraDitherFilter(256), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new SierraDitherFilter(128), 2));
        }

        [Test]
        public void TestStuckiDither()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "StuckiDither_2.bmp", new StuckiDitherFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "StuckiDither_4.bmp", new StuckiDitherFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new StuckiDitherFilter(256), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new StuckiDitherFilter(128), 2));
        }
    }
}
