using NUnit.Framework;
using FilterLib.Filters.Color;
using FilterLib.Util;
using System.Collections.Generic;
using FilterLib.Filters;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ColorTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("GradientMap_r_y_g.bmp", new GradientMapFilter(
                new Gradient(new RGB(255, 0, 0), new RGB(255, 255, 0), new RGB(0, 255, 0))), 1);
            yield return new TestCaseData("GradientMap_b_r_y.bmp", new GradientMapFilter(
                new Gradient(new RGB(0, 0, 255), new RGB(255, 0, 0), new RGB(255, 255, 0))), 1);
            yield return new TestCaseData("GradientMap_rainbow.bmp", new GradientMapFilter(
                new Gradient(
                    new RGB[]{
                        new RGB(255,0,0),
                        new RGB(255,0,255),
                        new RGB(0,0,255),
                        new RGB(0,255,255),
                        new RGB(0,255,0),
                        new RGB(255,255,0),
                        new RGB(255,0,0)},
                    new float[] { 1, 2, 3, 4, 5, 6, 7 })), 1);
        
            yield return new TestCaseData("Grayscale_100_0_0.bmp", new GrayscaleFilter(100, 0, 0), 1);
            yield return new TestCaseData("Grayscale_0_100_0.bmp", new GrayscaleFilter(0, 100, 0), 1);
            yield return new TestCaseData("Grayscale_0_0_100.bmp", new GrayscaleFilter(0, 0, 100), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", new GrayscaleFilter(30, 59, 11), 1);
        
            yield return new TestCaseData("Invert.bmp", new InvertFilter(), 0);
        
            yield return new TestCaseData("_input.bmp", new OrtonFilter(0, 3), 0);
            yield return new TestCaseData("Orton_80_5.bmp", new OrtonFilter(80, 5), 1);
        
            yield return new TestCaseData("Posterize_2.bmp", new PosterizeFilter(2), 1);
            yield return new TestCaseData("Posterize_4.bmp", new PosterizeFilter(4), 1);
            yield return new TestCaseData("Posterize_8.bmp", new PosterizeFilter(8), 1);
            yield return new TestCaseData("_input.bmp", new PosterizeFilter(256), 1);
        
            yield return new TestCaseData("Sepia.bmp", new SepiaFilter(), 1);

            yield return new TestCaseData("Solarize.bmp", new SolarizeFilter(), 0);

            yield return new TestCaseData("Treshold_63.bmp", new TresholdFilter(63), 1);
            yield return new TestCaseData("Treshold_127.bmp", new TresholdFilter(127), 1);
            yield return new TestCaseData("Treshold_191.bmp", new TresholdFilter(191), 1);
        
            yield return new TestCaseData("Vintage_80.bmp", new VintageFilter(80), 1);
            yield return new TestCaseData("_input.bmp", new VintageFilter(0), 1);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", expected, filter, tolerance));
    }
}
