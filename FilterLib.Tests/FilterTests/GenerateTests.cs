using FilterLib.Filters;
using FilterLib.Filters.Generate;
using FilterLib.Util;
using NUnit.Framework;
using System.Collections.Generic;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class GenerateTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("LinearGradientFilter_0pct_0pct_0pct_100pct.bmp",
                new LinearGradientFilter(Size.Relative(0), Size.Relative(0), Size.Relative(0), Size.Relative(1)), 1);
            yield return new TestCaseData("LinearGradientFilter_0pct_0pct_100pct_100pct.bmp",
                new LinearGradientFilter(Size.Relative(0), Size.Relative(0), Size.Relative(1), Size.Relative(1)), 1);
            yield return new TestCaseData("LinearGradientFilter_25pct_25pct_75pct_75pct.bmp",
                new LinearGradientFilter(Size.Relative(.25f), Size.Relative(.25f), Size.Relative(.75f), Size.Relative(.75f)), 1);

            yield return new TestCaseData("Marble_2_4_5_6_0.bmp", new MarbleFilter(2, 4, 5, 6, 0), 1);
            yield return new TestCaseData("Marble_3_7_2_5_0.bmp", new MarbleFilter(3, 7, 2, 5, 0), 1);
            yield return new TestCaseData("Marble_5_2_10_8_0.bmp", new MarbleFilter(5, 2, 10, 8, 0), 1);

            yield return new TestCaseData("RadialGradientFilter_50pct_50pct_0pct_50pct.bmp",
                new RadialGradientFilter(Size.Relative(.5f), Size.Relative(.5f), Size.Relative(0), Size.Relative(.5f)), 1);
            yield return new TestCaseData("RadialGradientFilter_10pct_25pct_25pct_80pct.bmp",
                new RadialGradientFilter(Size.Relative(.1f), Size.Relative(.25f), Size.Relative(.25f), Size.Relative(.8f)), 1);

            yield return new TestCaseData("Turbulence_1_0.bmp", new TurbulenceFilter(1, 0), 1);
            yield return new TestCaseData("Turbulence_5_0.bmp", new TurbulenceFilter(5, 0), 1);
            yield return new TestCaseData("Turbulence_10_0.bmp", new TurbulenceFilter(10, 0), 1);

            yield return new TestCaseData("WoodRings_2_0.05_8_0.bmp", new WoodRingsFilter(2, 0.05f, 8, 0), 1);
            yield return new TestCaseData("WoodRings_5_0.1_6_0.bmp", new WoodRingsFilter(5, 0.1f, 6, 0), 1);
            yield return new TestCaseData("WoodRings_8_0.3_7_0.bmp", new WoodRingsFilter(8, 0.3f, 7, 0), 1);
        }

        internal static IEnumerable<TestCaseData> Exceptions()
        {
            yield return new TestCaseData("_input.bmp", new LinearGradientFilter(Size.Absolute(0), Size.Absolute(0), Size.Absolute(0), Size.Absolute(0)));
            yield return new TestCaseData("_input.bmp", new RadialGradientFilter(Size.Absolute(0), Size.Absolute(0), Size.Absolute(1), Size.Absolute(0)));
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.That(Common.CheckFilter("_input.bmp", expected, filter, tolerance));

        [Test]
        [TestCaseSource("Exceptions")]
        public void TestEx(string expected, IFilter filter) =>
            Assert.Throws<System.ArgumentException>(() => Common.CheckFilter("_input.bmp", expected, filter));

    }
}
