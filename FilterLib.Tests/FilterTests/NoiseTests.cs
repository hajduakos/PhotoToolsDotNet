using FilterLib.Filters;
using FilterLib.Filters.Noise;
using NUnit.Framework;
using System.Collections.Generic;
using static FilterLib.Filters.Noise.AddNoiseFilter;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class NoiseTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("_input.bmp", new AddNoiseFilter(1000, 0, NoiseType.Monochrome, 0), 0);
            yield return new TestCaseData("_input.bmp", new AddNoiseFilter(0, 255, NoiseType.Color, 0), 0);
            yield return new TestCaseData("AddNoise_500_200_Color_0.bmp", new AddNoiseFilter(500, 200, NoiseType.Color, 0), 0);
            yield return new TestCaseData("AddNoise_500_200_Mono_0.bmp", new AddNoiseFilter(500, 200, NoiseType.Monochrome, 0), 0);
        
            yield return new TestCaseData("_input.bmp", new MedianFilter(0, 1), 0);
            yield return new TestCaseData("_input.bmp", new MedianFilter(80, 0), 0);
            yield return new TestCaseData("Median_80_1.bmp", new MedianFilter(80, 1), 0);
            yield return new TestCaseData("Median_50_3.bmp", new MedianFilter(50, 3), 0);
            yield return new TestCaseData("Median_100_5.bmp", new MedianFilter(100, 5), 0);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", expected, filter, tolerance));
    }
}
