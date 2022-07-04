using FilterLib.Filters;
using FilterLib.Filters.Mosaic;
using NUnit.Framework;
using System.Collections.Generic;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class MosaicTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("_input.bmp", new CrystallizeFilter(1, 100, 0), 1);
            yield return new TestCaseData("Crystallize_10_100_0.bmp", new CrystallizeFilter(10, 100, 0), 1);
            yield return new TestCaseData("Crystallize_30_50_0.bmp", new CrystallizeFilter(30, 50, 0), 1);

            yield return new TestCaseData("Lego_16_None.bmp", new LegoFilter(16, Util.AntiAliasQuality.None), 1);
            yield return new TestCaseData("Lego_32_None.bmp", new LegoFilter(32, Util.AntiAliasQuality.None), 1);
            yield return new TestCaseData("Lego_16_Med.bmp", new LegoFilter(16, Util.AntiAliasQuality.Medium), 1);
            yield return new TestCaseData("Lego_32_Med.bmp", new LegoFilter(32, Util.AntiAliasQuality.Medium), 1);
            yield return new TestCaseData("Lego_64_High.bmp", new LegoFilter(64, Util.AntiAliasQuality.High), 1);

            yield return new TestCaseData("_input.bmp", new PixelateFilter(1, PixelateFilter.PixelateMode.Average), 1);
            yield return new TestCaseData("Pixelate_10_A.bmp", new PixelateFilter(10, PixelateFilter.PixelateMode.Average), 1);
            yield return new TestCaseData("Pixelate_25_A.bmp", new PixelateFilter(25, PixelateFilter.PixelateMode.Average), 1);
            yield return new TestCaseData("_input.bmp", new PixelateFilter(1, PixelateFilter.PixelateMode.MidPoint), 1);
            yield return new TestCaseData("Pixelate_10_MP.bmp", new PixelateFilter(10, PixelateFilter.PixelateMode.MidPoint), 1);
            yield return new TestCaseData("Pixelate_25_MP.bmp", new PixelateFilter(25, PixelateFilter.PixelateMode.MidPoint), 1);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", expected, filter, tolerance));
    }
}

