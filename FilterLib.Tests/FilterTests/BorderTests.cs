using FilterLib.Filters.Border;
using FilterLib.Util;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class BorderTests
    {
        [Test]
        public void TestJitterBorder()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp",
                new JitterBorderFilter(Size.Absolute(0), new RGB(0, 0, 0), 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "JitterBorder_20_Red_0.bmp",
                new JitterBorderFilter(Size.Absolute(20), new RGB(255, 0, 0), 0), 1));
        }

        [Test]
        public void TestFadeBorder()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp",
                new FadeBorderFilter(Size.Absolute(0), new RGB(0, 0, 0)), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "FadeBorder_20_Red.bmp",
                new FadeBorderFilter(Size.Absolute(20), new RGB(255, 0, 0)), 1));
        }
    }
}
