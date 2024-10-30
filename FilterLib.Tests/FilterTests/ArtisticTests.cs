using FilterLib.Filters;
using FilterLib.Filters.Artistic;
using NUnit.Framework;
using System.Collections.Generic;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ArtisticTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("AdaptiveTreshold_1.bmp", new AdaptiveTresholdFilter(1), 1);
            yield return new TestCaseData("AdaptiveTreshold_4.bmp", new AdaptiveTresholdFilter(4), 1);
            yield return new TestCaseData("AdaptiveTreshold_8.bmp", new AdaptiveTresholdFilter(8), 1);
            yield return new TestCaseData("AdaptiveTreshold_200.bmp", new AdaptiveTresholdFilter(200), 1);

            yield return new TestCaseData("_input.bmp", new OilPaintFilter(0, 255), 0);
            yield return new TestCaseData("OilPaint_1_255.bmp", new OilPaintFilter(1, 255), 1);
            yield return new TestCaseData("OilPaint_5_255.bmp", new OilPaintFilter(5, 255), 1);
            yield return new TestCaseData("OilPaint_10_255.bmp", new OilPaintFilter(10, 255), 1);
            yield return new TestCaseData("OilPaint_1_128.bmp", new OilPaintFilter(1, 128), 1);
            yield return new TestCaseData("OilPaint_5_64.bmp", new OilPaintFilter(5, 64), 1);
            yield return new TestCaseData("OilPaint_10_32.bmp", new OilPaintFilter(10, 32), 1);

            yield return new TestCaseData("_input.bmp", new RandomJitterFilter(0, 0), 0);
            yield return new TestCaseData("RandomJitter_1_0.bmp", new RandomJitterFilter(1, 0), 0);
            yield return new TestCaseData("RandomJitter_5_0.bmp", new RandomJitterFilter(5, 0), 0);
            yield return new TestCaseData("RandomJitter_10_0.bmp", new RandomJitterFilter(10, 0), 0);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.That(Common.CheckFilter("_input.bmp", expected, filter, tolerance));
    }
}
