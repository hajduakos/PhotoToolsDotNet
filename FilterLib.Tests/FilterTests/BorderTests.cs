using FilterLib.Filters.Border;
using FilterLib.Util;
using NUnit.Framework;
using Bitmap = System.Drawing.Bitmap;
using System.Runtime.InteropServices;
using FilterLib.Filters;
using System.Collections.Generic;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    public class BorderTests
    {
        private static readonly Bitmap pattern = new(TestContext.CurrentContext.TestDirectory + "/TestImages/_input2.bmp");
        internal static IEnumerable<TestCaseData> Data()
        {
            string suffix = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "" : "_l";

            yield return new TestCaseData("_input.bmp",
                new JitterBorderFilter(Size.Absolute(0), new RGB(0, 0, 0), 0), 1);
            yield return new TestCaseData("JitterBorder_20_Red_0.bmp",
                new JitterBorderFilter(Size.Absolute(20), new RGB(255, 0, 0), 0), 1);
        
            yield return new TestCaseData("_input.bmp",
                new FadeBorderFilter(Size.Absolute(0), new RGB(0, 0, 0)), 1);
            yield return new TestCaseData("FadeBorder_20_Red.bmp",
                new FadeBorderFilter(Size.Absolute(20), new RGB(255, 0, 0)), 1);

            yield return new TestCaseData("_input.bmp",
                new PatternBorderFilter(Size.Absolute(0), Size.Absolute(0), pattern, BorderPosition.Inside), 1);
            yield return new TestCaseData("PatternBorder_30px_0_Green_Inside.bmp",
                new PatternBorderFilter(Size.Absolute(30), Size.Absolute(0), pattern, BorderPosition.Inside), 1);
        
            yield return new TestCaseData($"PatternBorder_10pct_8px_Outside{suffix}.bmp",
                    new PatternBorderFilter(Size.Relative(.1f), Size.Absolute(8), pattern, BorderPosition.Outside), 1);
            yield return new TestCaseData($"PatternBorder_20px_10pct_Center{suffix}.bmp",
                new PatternBorderFilter(Size.Absolute(20), Size.Relative(.1f), pattern, BorderPosition.Center), 1);
        
            yield return new TestCaseData("_input.bmp",
                new SimpleBorderFilter(Size.Absolute(0), Size.Absolute(0), new RGB(0, 0, 0), BorderPosition.Inside), 1);
            yield return new TestCaseData("SimpleBorder_30px_0_Green_Inside.bmp",
                new SimpleBorderFilter(Size.Absolute(30), Size.Absolute(0), new RGB(0, 255, 0), BorderPosition.Inside), 1);
        
            yield return new TestCaseData($"SimpleBorder_10pct_8px_Blue_Outside{suffix}.bmp",
                    new SimpleBorderFilter(Size.Relative(.1f), Size.Absolute(8), new RGB(0, 0, 255), BorderPosition.Outside), 1);
            yield return new TestCaseData($"SimpleBorder_20px_10pct_Red_Center{suffix}.bmp",
                new SimpleBorderFilter(Size.Absolute(20), Size.Relative(.1f), new RGB(255, 0, 0), BorderPosition.Center), 1);
        
            yield return new TestCaseData("_input.bmp", new VignetteFilter(Size.Relative(3), Size.Relative(2)), 1);
            yield return new TestCaseData("Vignette_150pct_60pct.bmp", new VignetteFilter(Size.Relative(1.5f), Size.Relative(0.6f)), 1);
            yield return new TestCaseData("Vignette_150pct_60pct.bmp", new VignetteFilter(Size.Absolute(120), Size.Absolute(48)), 1);
            yield return new TestCaseData("Vignette_100pct_90pct.bmp", new VignetteFilter(Size.Relative(1f), Size.Relative(0.9f)), 1);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", expected, filter, tolerance));

        [OneTimeTearDown]
        public static void CleanUp() => pattern.Dispose();
    }
}
