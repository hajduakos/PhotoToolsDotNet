using FilterLib.Filters.Mosaic;
using NUnit.Framework;

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
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Lego_16.bmp", new LegoFilter(16), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Lego_32.bmp", new LegoFilter(32), 1));
        }
    }
}
