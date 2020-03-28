using FilterLib.Filters.Noise;
using NUnit.Framework;
using static FilterLib.Filters.Noise.AddNoiseFilter;

namespace FilterLib.Tests.FilterTests
{
    public class NoiseTests
    {
        [Test]
        public void TestAddNoise()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new AddNoiseFilter(1000, 0, NoiseType.Monochrome, 0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new AddNoiseFilter(0, 255, NoiseType.Color, 0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "AddNoise_500_200_Color_0.bmp", new AddNoiseFilter(500, 200, NoiseType.Color, 0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "AddNoise_500_200_Mono_0.bmp", new AddNoiseFilter(500, 200, NoiseType.Monochrome, 0), 0));
        }

        [Test]
        public void TestMedian()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new MedianFilter(0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Median_80.bmp", new MedianFilter(80), 0));
        }
    }
}
