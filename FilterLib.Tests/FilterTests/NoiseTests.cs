using FilterLib.Filters.Noise;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class NoiseTests
    {
        [Test]
        public void TestAddNoise()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new AddNoiseFilter(1000, 0, true, 0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new AddNoiseFilter(0, 255, false, 0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "AddNoise_500_200_False_0.bmp", new AddNoiseFilter(500, 200, false, 0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "AddNoise_500_200_True_0.bmp", new AddNoiseFilter(500, 200, true, 0), 0));
        }

        [Test]
        public void TestMedian()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new MedianFilter(0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Median_80.bmp", new MedianFilter(80), 0));
        }
    }
}
