using FilterLib.Filters;
using FilterLib.Filters.Mosaic;
using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class MosaicTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            string suffix = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "" : "_l";

            yield return new TestCaseData("Crystallize_10_100_0.bmp", new CrystallizeFilter(10, 100, 0), 1);
            yield return new TestCaseData("Crystallize_30_50_0.bmp", new CrystallizeFilter(30, 50, 0), 1);

            yield return new TestCaseData($"Lego_16{suffix}.bmp", new LegoFilter(16), 1);
            yield return new TestCaseData($"Lego_32{suffix}.bmp", new LegoFilter(32), 1);

            yield return new TestCaseData("_input.bmp", new PixelateFilter(1), 1);
            yield return new TestCaseData("Pixelate_10.bmp", new PixelateFilter(10), 1);
            yield return new TestCaseData("Pixelate_25.bmp", new PixelateFilter(25), 1);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", expected, filter, tolerance));
    }
}

