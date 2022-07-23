using FilterLib.Filters;
using FilterLib.Filters.Blur;
using NUnit.Framework;
using System.Collections.Generic;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class BlurTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("Blur.bmp", new BlurFilter(), 1);

            yield return new TestCaseData("_input.bmp", new BoxBlurFilter(0, 0), 1);
            yield return new TestCaseData("BoxBlur_5_0.bmp", new BoxBlurFilter(5, 0), 1);
            yield return new TestCaseData("BoxBlur_0_5.bmp", new BoxBlurFilter(0, 5), 1);
            yield return new TestCaseData("BoxBlur_10_20.bmp", new BoxBlurFilter(10, 20), 1);
            yield return new TestCaseData("BoxBlur_200_300.bmp", new BoxBlurFilter(200, 300), 1);

            yield return new TestCaseData("_input.bmp", new GaussianBlurFilter(0), 1);
            yield return new TestCaseData("GaussianBlur_5.bmp", new GaussianBlurFilter(5), 1);

            yield return new TestCaseData("_input.bmp", new MotionBlurFilter(0, 0), 1);
            yield return new TestCaseData("MotionBlur_10_30.bmp", new MotionBlurFilter(10, 30), 1);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", expected, filter, tolerance));
    }
}
