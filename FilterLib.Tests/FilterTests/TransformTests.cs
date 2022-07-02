using FilterLib.Filters;
using FilterLib.Filters.Transform;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using InterpolationMode = FilterLib.Filters.Transform.InterpolationMode;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class TransformTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Util.Size.Absolute(0), Util.Size.Absolute(0), Util.Size.Relative(1), Util.Size.Relative(1)), 0);
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Util.Size.Absolute(0), Util.Size.Absolute(0), Util.Size.Relative(2), Util.Size.Relative(2)), 0);
            yield return new TestCaseData("Crop_25pct_45px_50pct_40px.bmp",
                new CropFilter(Util.Size.Relative(.25f), Util.Size.Absolute(45), Util.Size.Relative(.5f), Util.Size.Absolute(40)), 0);

            yield return new TestCaseData("FlipHorizontal.bmp", new FlipHorizontalFilter(), 0);

            yield return new TestCaseData("FlipVertical.bmp", new FlipVerticalFilter(), 0);

            yield return new TestCaseData("_input.bmp",
                new ResizeFilter(Util.Size.Relative(1), Util.Size.Relative(1), InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Resize_200pct_30px_NN.bmp",
                new ResizeFilter(Util.Size.Relative(2), Util.Size.Absolute(30), InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Resize_25pct_50pct_NN.bmp",
                new ResizeFilter(Util.Size.Relative(.25f), Util.Size.Relative(.5f), InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("_input.bmp",
                new ResizeFilter(Util.Size.Relative(1), Util.Size.Relative(1), InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Resize_200pct_30px_BL.bmp",
                new ResizeFilter(Util.Size.Relative(2), Util.Size.Absolute(30), InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Resize_25pct_50pct_BL.bmp",
                new ResizeFilter(Util.Size.Relative(.25f), Util.Size.Relative(.5f), InterpolationMode.Bilinear), 1);

            yield return new TestCaseData("_input.bmp", new RotateFilter(0, false, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("_input.bmp", new RotateFilter(0, true, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Rotate_30_false_NN.bmp", new RotateFilter(30, false, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Rotate_30_true_NN.bmp", new RotateFilter(30, true, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("_input.bmp", new RotateFilter(0, false, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("_input.bmp", new RotateFilter(0, true, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Rotate_30_false_BL.bmp", new RotateFilter(30, false, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Rotate_30_true_BL.bmp", new RotateFilter(30, true, InterpolationMode.Bilinear), 1);

            yield return new TestCaseData("Rotate180.bmp", new Rotate180Filter(), 0);

            yield return new TestCaseData("RotateLeft.bmp", new RotateLeftFilter(), 0);

            yield return new TestCaseData("RotateRight.bmp", new RotateRightFilter(), 0);
        }

        internal static IEnumerable<TestCaseData> Exceptions()
        {
            yield return new TestCaseData("_input.bmp",
                    new CropFilter(Util.Size.Absolute(-1), Util.Size.Absolute(0), Util.Size.Relative(1), Util.Size.Relative(1)));
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Util.Size.Absolute(0), Util.Size.Absolute(-1), Util.Size.Relative(1), Util.Size.Relative(1)));
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Util.Size.Absolute(0), Util.Size.Absolute(0), Util.Size.Relative(0), Util.Size.Relative(1)));
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Util.Size.Absolute(0), Util.Size.Absolute(0), Util.Size.Relative(1), Util.Size.Relative(0)));

            yield return new TestCaseData("_input.bmp",
                new ResizeFilter(Util.Size.Relative(0), Util.Size.Relative(1), InterpolationMode.NearestNeighbor));
            yield return new TestCaseData("_input.bmp",
                new ResizeFilter(Util.Size.Relative(1), Util.Size.Relative(0), InterpolationMode.NearestNeighbor));
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", expected, filter, tolerance));

        [Test]
        [TestCaseSource("Exceptions")]
        public void TestEx(string expected, IFilter filter) =>
            Assert.Throws<ArgumentException>(() => Common.CheckFilter("_input.bmp", expected, filter));
    }
}
