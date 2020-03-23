using FilterLib.Filters.Mosaic;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class MosaicTests
    {
        [Test]
        public void TestMosaic()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Crystallize_10_100_0.bmp", new CrystallizeFilter(10, 100, 0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Crystallize_30_50_0.bmp", new CrystallizeFilter(30, 50, 0), 0));
        }
    }
}
