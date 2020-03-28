using FilterLib.Filters.Mosaic;
using NUnit.Framework;
using System.Runtime.InteropServices;

namespace FilterLib.Tests.FilterTests
{
    public class MosaicTests
    {
        [Test]
        public void TestCrystallize()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Crystallize_10_100_0.bmp", new CrystallizeFilter(10, 100, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Crystallize_30_50_0.bmp", new CrystallizeFilter(30, 50, 0), 1));
        }

        [Test]
        public void TestLego()
        {
            string suffix = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "" : "l";
            Assert.IsTrue(Common.CheckFilter("_input.bmp", $"Lego_16{suffix}.bmp", new LegoFilter(16), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", $"Lego_32{suffix}.bmp", new LegoFilter(32), 1));
        }

        [Test]
        public void TestPixelate()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new PixelateFilter(1), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Pixelate_10.bmp", new PixelateFilter(10), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Pixelate_25.bmp", new PixelateFilter(25), 1));
        }
    }
}

