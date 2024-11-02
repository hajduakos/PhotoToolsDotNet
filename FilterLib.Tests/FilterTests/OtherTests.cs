using FilterLib.Filters;
using FilterLib.Filters.Other;
using FilterLib.Filters.Transform;
using FilterLib.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class OtherTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("_input_eq.bmp", "ConvertToPolar_72.bmp", new ConvertToPolarFilter(72), 1);

            yield return new TestCaseData("_input.bmp", "_input.bmp",
                new ConvolutionFilter(new ConvolutionMatrix(new int[,] { { 1, }, }, 1, 0)), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp",
                new ConvolutionFilter(new ConvolutionMatrix(new int[,] { { 0, 1, 0 } }, 1, 0)), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp",
                new ConvolutionFilter(new ConvolutionMatrix(new int[,] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } }, 1, 0)), 1);
            yield return new TestCaseData("_input.bmp", "Convolution_1_0_-1_1_0_-1_1_0_-1_2_30.bmp",
                new ConvolutionFilter(new ConvolutionMatrix(new int[,] { { 1, 0, -1 }, { 1, 0, -1 }, { 1, 0, -1 } }, 2, 30)), 1);
        
            yield return new TestCaseData("_input_eq.bmp", "EquirectToStereo_100_0_BL.bmp",
                new EquirectangularToStereographicFilter(100, 0, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("_input_eq.bmp", "EquirectToStereo_120_150_BL.bmp",
                new EquirectangularToStereographicFilter(120, 150, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("_input_eq.bmp", "EquirectToStereo_80_300_NN.bmp",
                new EquirectangularToStereographicFilter(80, 300, InterpolationMode.NearestNeighbor), 1);

            yield return new TestCaseData("_input.bmp", "_input.bmp",
                new WavesFilter(Size.Relative(1), Size.Absolute(0), WavesFilter.WaveDirection.Horizontal), 1);
            yield return new TestCaseData("_input.bmp", "_input.bmp",
                new WavesFilter(Size.Relative(.5f), Size.Absolute(0), WavesFilter.WaveDirection.Vertical), 1);
            yield return new TestCaseData("_input.bmp", "Waves_50pct_9px_Horizontal.bmp",
                new WavesFilter(Size.Relative(.5f), Size.Absolute(9), WavesFilter.WaveDirection.Horizontal), 1);
            yield return new TestCaseData("_input.bmp", "Waves_10pct_16px_Vertical.bmp",
                new WavesFilter(Size.Relative(.1f), Size.Absolute(16), WavesFilter.WaveDirection.Vertical), 1);
        }
        internal static IEnumerable<TestCaseData> Exceptions()
        {
            yield return new TestCaseData("_input.bmp", "_input.bmp",
                new WavesFilter(Size.Absolute(0), Size.Relative(1), WavesFilter.WaveDirection.Horizontal));
            yield return new TestCaseData("_input.bmp", "_input.bmp",
                new WavesFilter(Size.Relative(0), Size.Relative(1), WavesFilter.WaveDirection.Horizontal));
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string input, string expected, IFilter filter, int tolerance) =>
            Assert.That(Common.CheckFilter(input, expected, filter, tolerance));

        [Test]
        [TestCaseSource("Exceptions")]
        public void TestEx(string input, string expected, IFilter filter) =>
            Assert.Throws<ArgumentException>(() => Common.CheckFilter(input, expected, filter));
    }
}
