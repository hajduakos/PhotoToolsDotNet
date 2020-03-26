using FilterLib.Other;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class OtherTests
    {
        [Test]
        public void TestConvolution()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp",
                new ConvolutionFilter(new Util.Conv3x3(0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0)), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Convolution_1_1_1_0_0_0_-1_-1_-1_2_30.bmp",
                new ConvolutionFilter(new Util.Conv3x3(1, 1, 1, 0, 0, 0, -1, -1, -1, 2, 30)), 1));
        }
    }
}
