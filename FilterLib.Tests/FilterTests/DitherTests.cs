using FilterLib.Filters;
using FilterLib.Filters.Dither;
using NUnit.Framework;
using System.Collections.Generic;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class DitherTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("_input.bmp", "BayerDither_2_1.bmp", new BayerDitherFilter(2, 1), 1);
            yield return new TestCaseData("_input.bmp", "BayerDither_2_2.bmp", new BayerDitherFilter(2, 2), 1);
            yield return new TestCaseData("_input.bmp", "BayerDither_2_5.bmp", new BayerDitherFilter(2, 5), 1);
            yield return new TestCaseData("_input.bmp", "BayerDither_4_4.bmp", new BayerDitherFilter(4, 4), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new BayerDitherFilter(256, 1), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new BayerDitherFilter(256, 2), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new BayerDitherFilter(256, 4), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new BayerDitherFilter(128, 4), 2);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new BayerDitherFilter(64, 4), 5);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "BayerDither_2_1_bw.bmp", new BayerDitherFilter(2, 1), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "BayerDither_2_2_bw.bmp", new BayerDitherFilter(2, 2), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "BayerDither_2_5_bw.bmp", new BayerDitherFilter(2, 5), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "BayerDither_4_4_bw.bmp", new BayerDitherFilter(4, 4), 1);

            yield return new TestCaseData("_input.bmp", "AtkinsonDither_2.bmp", new AtkinsonDitherFilter(2), 1);
            yield return new TestCaseData("_input.bmp", "AtkinsonDither_4.bmp", new AtkinsonDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new AtkinsonDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new AtkinsonDitherFilter(128), 2);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "AtkinsonDither_2_bw.bmp", new AtkinsonDitherFilter(2), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "AtkinsonDither_4_bw.bmp", new AtkinsonDitherFilter(4), 1);

            yield return new TestCaseData("_input.bmp", "BurkesDither_2.bmp", new BurkesDitherFilter(2), 1);
            yield return new TestCaseData("_input.bmp", "BurkesDither_4.bmp", new BurkesDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new BurkesDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new BurkesDitherFilter(128), 2);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "BurkesDither_2_bw.bmp", new BurkesDitherFilter(2), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "BurkesDither_4_bw.bmp", new BurkesDitherFilter(4), 1);

            yield return new TestCaseData("_input.bmp", "FanDither_2.bmp", new FanDitherFilter(2), 1);
            yield return new TestCaseData("_input.bmp", "FanDither_4.bmp", new FanDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new FanDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new FanDitherFilter(128), 2);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "FanDither_2_bw.bmp", new FanDitherFilter(2), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "FanDither_4_bw.bmp", new FanDitherFilter(4), 1);

            yield return new TestCaseData("_input.bmp", "ShiauFanDither_2.bmp", new ShiauFanDitherFilter(2), 1);
            yield return new TestCaseData("_input.bmp", "ShiauFanDither_4.bmp", new ShiauFanDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new ShiauFanDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new ShiauFanDitherFilter(128), 2);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "ShiauFanDither_2_bw.bmp", new ShiauFanDitherFilter(2), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "ShiauFanDither_4_bw.bmp", new ShiauFanDitherFilter(4), 1);

            yield return new TestCaseData("_input.bmp", "FloydSteinbergDither_2.bmp", new FloydSteinbergDitherFilter(2), 1);
            yield return new TestCaseData("_input.bmp", "FloydSteinbergDither_4.bmp", new FloydSteinbergDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new FloydSteinbergDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new FloydSteinbergDitherFilter(128), 2);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "FloydSteinbergDither_2_bw.bmp", new FloydSteinbergDitherFilter(2), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "FloydSteinbergDither_4_bw.bmp", new FloydSteinbergDitherFilter(4), 1);

            yield return new TestCaseData("_input.bmp", "JarvisJudiceNinkeDither_2.bmp", new JarvisJudiceNinkeDitherFilter(2), 1);
            yield return new TestCaseData("_input.bmp", "JarvisJudiceNinkeDither_4.bmp", new JarvisJudiceNinkeDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new JarvisJudiceNinkeDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new JarvisJudiceNinkeDitherFilter(128), 2);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "JarvisJudiceNinkeDither_2_bw.bmp", new JarvisJudiceNinkeDitherFilter(2), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "JarvisJudiceNinkeDither_4_bw.bmp", new JarvisJudiceNinkeDitherFilter(4), 1);

            yield return new TestCaseData("_input.bmp", "RandomDither_2.bmp", new RandomDitherFilter(2), 1);
            yield return new TestCaseData("_input.bmp", "RandomDither_4.bmp", new RandomDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new RandomDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new RandomDitherFilter(128), 2);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "RandomDither_2_bw.bmp", new RandomDitherFilter(2), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "RandomDither_4_bw.bmp", new RandomDitherFilter(4), 1);

            yield return new TestCaseData("_input.bmp", "SierraDither_2.bmp", new SierraDitherFilter(2), 1);
            yield return new TestCaseData("_input.bmp", "SierraDither_4.bmp", new SierraDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new SierraDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new SierraDitherFilter(128), 2);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "SierraDither_2_bw.bmp", new SierraDitherFilter(2), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "SierraDither_4_bw.bmp", new SierraDitherFilter(4), 1);

            yield return new TestCaseData("_input.bmp", "StuckiDither_2.bmp", new StuckiDitherFilter(2), 1);
            yield return new TestCaseData("_input.bmp", "StuckiDither_4.bmp", new StuckiDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new StuckiDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp", new StuckiDitherFilter(128), 2);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "StuckiDither_2_bw.bmp", new StuckiDitherFilter(2), 1);
            yield return new TestCaseData("Grayscale_30_59_11.bmp", "StuckiDither_4_bw.bmp", new StuckiDitherFilter(4), 1);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string input, string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter(input, expected, filter, tolerance));
    }
}
