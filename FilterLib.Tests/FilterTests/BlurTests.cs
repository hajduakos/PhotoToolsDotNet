using FilterLib.Filters;
using FilterLib.Filters.Blur;
using FilterLib.Util;
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

            yield return new TestCaseData("_input.bmp", new SpinBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 0, 2), 1);
            yield return new TestCaseData("SpinBlur_0pct_0pct_10_5.bmp", new SpinBlurFilter(Size.Relative(0), Size.Relative(0), 10, 5), 1);
            yield return new TestCaseData("SpinBlur_50pct_50pct_10_5.bmp", new SpinBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 10, 5), 1);
            yield return new TestCaseData("SpinBlur_50pct_50pct_30_20.bmp", new SpinBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 30, 20), 1);
            yield return new TestCaseData("SpinBlur_50pct_50pct_180_50.bmp", new SpinBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 180, 50), 1);

            yield return new TestCaseData("_input.bmp", new ZoomBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 0), 1);
            yield return new TestCaseData("ZoomBlur_50pct_50pct_20.bmp", new ZoomBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 20), 1);
            yield return new TestCaseData("ZoomBlur_50pct_50pct_50.bmp", new ZoomBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 50), 1);
            yield return new TestCaseData("ZoomBlur_50pct_50pct_100.bmp", new ZoomBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 100), 1);
            yield return new TestCaseData("ZoomBlur_100pct_100pct_30.bmp", new ZoomBlurFilter(Size.Relative(1), Size.Relative(1), 30), 1);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", expected, filter, tolerance));
    }
}
