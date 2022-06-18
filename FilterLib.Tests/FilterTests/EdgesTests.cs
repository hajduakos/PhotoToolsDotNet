using FilterLib.Filters;
using FilterLib.Filters.Sharpen;
using NUnit.Framework;
using System.Collections.Generic;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class EdgesTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("EdgeDetection.bmp", new EdgeDetectionFilter(), 1);

            yield return new TestCaseData("Emboss.bmp", new EmbossFilter(), 1);

            yield return new TestCaseData("Prewitt.bmp", new PrewittFilter(), 1);

            yield return new TestCaseData("Sobel.bmp", new SobelFilter(), 1);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", expected, filter, tolerance));
    }
}
