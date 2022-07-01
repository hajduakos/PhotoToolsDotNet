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
            yield return new TestCaseData("BayerDither_2_1.bmp", new BayerDitherFilter(2, 1), 1);
            yield return new TestCaseData("BayerDither_2_2.bmp", new BayerDitherFilter(2, 2), 1);
            yield return new TestCaseData("BayerDither_2_5.bmp", new BayerDitherFilter(2, 5), 1);
            yield return new TestCaseData("BayerDither_4_4.bmp", new BayerDitherFilter(4, 4), 1);
            yield return new TestCaseData("_input.bmp", new BayerDitherFilter(256, 1), 1);
            yield return new TestCaseData("_input.bmp", new BayerDitherFilter(256, 2), 1);
            yield return new TestCaseData("_input.bmp", new BayerDitherFilter(256, 4), 1);
            yield return new TestCaseData("_input.bmp", new BayerDitherFilter(128, 4), 2);
            yield return new TestCaseData("_input.bmp", new BayerDitherFilter(64, 4), 5);
        
            yield return new TestCaseData("AtkinsonDither_2.bmp", new AtkinsonDitherFilter(2), 1);
            yield return new TestCaseData("AtkinsonDither_4.bmp", new AtkinsonDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", new AtkinsonDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", new AtkinsonDitherFilter(128), 2);
        
            yield return new TestCaseData("BurkesDither_2.bmp", new BurkesDitherFilter(2), 1);
            yield return new TestCaseData("BurkesDither_4.bmp", new BurkesDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", new BurkesDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", new BurkesDitherFilter(128), 2);
        
            yield return new TestCaseData("FanDither_2.bmp", new FanDitherFilter(2), 1);
            yield return new TestCaseData("FanDither_4.bmp", new FanDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", new FanDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", new FanDitherFilter(128), 2);
        
            yield return new TestCaseData("ShiauFanDither_2.bmp", new ShiauFanDitherFilter(2), 1);
            yield return new TestCaseData("ShiauFanDither_4.bmp", new ShiauFanDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", new ShiauFanDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", new ShiauFanDitherFilter(128), 2);
        
            yield return new TestCaseData("FloydSteinbergDither_2.bmp", new FloydSteinbergDitherFilter(2), 1);
            yield return new TestCaseData("FloydSteinbergDither_4.bmp", new FloydSteinbergDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", new FloydSteinbergDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", new FloydSteinbergDitherFilter(128), 2);
        
            yield return new TestCaseData("JarvisJudiceNinkeDither_2.bmp", new JarvisJudiceNinkeDitherFilter(2), 1);
            yield return new TestCaseData("JarvisJudiceNinkeDither_4.bmp", new JarvisJudiceNinkeDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", new JarvisJudiceNinkeDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", new JarvisJudiceNinkeDitherFilter(128), 2);
        
            yield return new TestCaseData("RandomDither_2.bmp", new RandomDitherFilter(2), 1);
            yield return new TestCaseData("RandomDither_4.bmp", new RandomDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", new RandomDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", new RandomDitherFilter(128), 2);
        
            yield return new TestCaseData("SierraDither_2.bmp", new SierraDitherFilter(2), 1);
            yield return new TestCaseData("SierraDither_4.bmp", new SierraDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", new SierraDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", new SierraDitherFilter(128), 2);
        
            yield return new TestCaseData("StuckiDither_2.bmp", new StuckiDitherFilter(2), 1);
            yield return new TestCaseData("StuckiDither_4.bmp", new StuckiDitherFilter(4), 1);
            yield return new TestCaseData("_input.bmp", new StuckiDitherFilter(256), 1);
            yield return new TestCaseData("_input.bmp", new StuckiDitherFilter(128), 2);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", expected, filter, tolerance));
    }
}
