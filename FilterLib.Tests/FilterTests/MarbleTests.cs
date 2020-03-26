using FilterLib.Filters.Generate;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class MarbleTests
    {
        [Test]
        public void TestMarble()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Marble_2_4_5_6_0.bmp", new MarbleFilter(2, 4, 5, 6, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Marble_3_7_2_5_0.bmp", new MarbleFilter(3, 7, 2, 5, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Marble_5_2_10_8_0.bmp", new MarbleFilter(5, 2, 10, 8, 0), 1));
        }
    }
}
