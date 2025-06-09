using FilterLib.Filters;
using FilterLib.Filters.Transform;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using CropMode = FilterLib.Filters.Transform.RotateFilter.CropMode;
using InterpolationMode = FilterLib.Util.InterpolationMode;
using Size = FilterLib.Util.Size;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class TransformTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("_input.bmp",
                new BoxDownscaleFilter(Size.Relative(1), Size.Relative(1)), 1);
            yield return new TestCaseData("BoxDownscale_19px_13px.bmp",
                new BoxDownscaleFilter(Size.Absolute(19), Size.Absolute(13)), 1);
            yield return new TestCaseData("BoxDownscale_20pct_10pct.bmp",
                new BoxDownscaleFilter(Size.Relative(.2f), Size.Relative(.1f)), 1);
            yield return new TestCaseData("BoxDownscale_50pct_50pct.bmp",
                new BoxDownscaleFilter(Size.Relative(.5f), Size.Relative(.5f)), 1);

            yield return new TestCaseData("_input.bmp",
                new CropFilter(Size.Absolute(0), Size.Absolute(0), Size.Relative(1), Size.Relative(1)), 0);
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Size.Absolute(0), Size.Absolute(0), Size.Relative(2), Size.Relative(2)), 0);
            yield return new TestCaseData("Crop_25pct_45px_50pct_40px.bmp",
                new CropFilter(Size.Relative(.25f), Size.Absolute(45), Size.Relative(.5f), Size.Absolute(40)), 0);

            yield return new TestCaseData("FlipHorizontal.bmp", new FlipHorizontalFilter(), 0);

            yield return new TestCaseData("FlipVertical.bmp", new FlipVerticalFilter(), 0);

            yield return new TestCaseData("_input.bmp", new PerspectiveFilter(1f, PerspectiveFilter.PerspectiveDirection.Horizontal, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Perspective_1.5_Horizontal_BL.bmp", new PerspectiveFilter(1.5f, PerspectiveFilter.PerspectiveDirection.Horizontal, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Perspective_0.5_Horizontal_NN.bmp", new PerspectiveFilter(0.5f, PerspectiveFilter.PerspectiveDirection.Horizontal, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Perspective_-1.8_Horizontal_NN.bmp", new PerspectiveFilter(-1.8f, PerspectiveFilter.PerspectiveDirection.Horizontal, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Perspective_-0.8_Horizontal_BL.bmp", new PerspectiveFilter(-0.8f, PerspectiveFilter.PerspectiveDirection.Horizontal, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("_input.bmp", new PerspectiveFilter(1f, PerspectiveFilter.PerspectiveDirection.Vertical), 1);
            yield return new TestCaseData("Perspective_1.5_Vertical_BL.bmp", new PerspectiveFilter(1.5f, PerspectiveFilter.PerspectiveDirection.Vertical, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Perspective_0.5_Vertical_NN.bmp", new PerspectiveFilter(0.5f, PerspectiveFilter.PerspectiveDirection.Vertical, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Perspective_-1.8_Vertical_NN.bmp", new PerspectiveFilter(-1.8f, PerspectiveFilter.PerspectiveDirection.Vertical, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Perspective_-0.8_Vertical_BL.bmp", new PerspectiveFilter(-0.8f, PerspectiveFilter.PerspectiveDirection.Vertical, InterpolationMode.Bilinear), 1);

            yield return new TestCaseData("_input.bmp",
                new ResizeFilter(Size.Relative(1), Size.Relative(1), InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Resize_200pct_30px_NN.bmp",
                new ResizeFilter(Size.Relative(2), Size.Absolute(30), InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Resize_25pct_50pct_NN.bmp",
                new ResizeFilter(Size.Relative(.25f), Size.Relative(.5f), InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("_input.bmp",
                new ResizeFilter(Size.Relative(1), Size.Relative(1), InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Resize_200pct_30px_BL.bmp",
                new ResizeFilter(Size.Relative(2), Size.Absolute(30), InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Resize_25pct_50pct_BL.bmp",
                new ResizeFilter(Size.Relative(.25f), Size.Relative(.5f), InterpolationMode.Bilinear), 1);

            yield return new TestCaseData("_input.bmp", new RotateFilter(0, CropMode.Fit, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("_input.bmp", new RotateFilter(0, CropMode.Fill, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Rotate_30_Fit_NN.bmp", new RotateFilter(30, CropMode.Fit, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Rotate_30_Fill_NN.bmp", new RotateFilter(30, CropMode.Fill, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Rotate_60_Fit_NN.bmp", new RotateFilter(60, CropMode.Fit, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Rotate_60_Fill_NN.bmp", new RotateFilter(60, CropMode.Fill, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("_input.bmp", new RotateFilter(0, CropMode.Fit, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("_input.bmp", new RotateFilter(0, CropMode.Fill, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Rotate_30_Fit_BL.bmp", new RotateFilter(30, CropMode.Fit, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Rotate_30_Fill_BL.bmp", new RotateFilter(30, CropMode.Fill, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Rotate_210_Fit_BL.bmp", new RotateFilter(210, CropMode.Fit, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Rotate_210_Fill_BL.bmp", new RotateFilter(210, CropMode.Fill, InterpolationMode.Bilinear), 1);

            yield return new TestCaseData("Rotate180.bmp", new Rotate180Filter(), 0);

            yield return new TestCaseData("RotateLeft.bmp", new RotateLeftFilter(), 0);

            yield return new TestCaseData("RotateRight.bmp", new RotateRightFilter(), 0);

            yield return new TestCaseData("_input.bmp", new SkewFilter(0, SkewFilter.SkewDirection.Horizontal), 1);
            yield return new TestCaseData("_input.bmp", new SkewFilter(0, SkewFilter.SkewDirection.Vertical), 1);
            yield return new TestCaseData("Skew_30_Horizontal_BL.bmp", new SkewFilter(30, SkewFilter.SkewDirection.Horizontal, InterpolationMode.Bilinear), 1);
            yield return new TestCaseData("Skew_-45_Horizontal_NN.bmp", new SkewFilter(-45, SkewFilter.SkewDirection.Horizontal, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Skew_30_Vertical_NN.bmp", new SkewFilter(30, SkewFilter.SkewDirection.Vertical, InterpolationMode.NearestNeighbor), 1);
            yield return new TestCaseData("Skew_-10_Vertical_BL.bmp", new SkewFilter(-10, SkewFilter.SkewDirection.Vertical, InterpolationMode.Bilinear), 1);
        }

        internal static IEnumerable<TestCaseData> Exceptions()
        {
            yield return new TestCaseData("_input.bmp",
                new BoxDownscaleFilter(Size.Relative(0), Size.Relative(1)));
            yield return new TestCaseData("_input.bmp",
                new BoxDownscaleFilter(Size.Relative(1), Size.Relative(0)));
            yield return new TestCaseData("_input.bmp",
                new BoxDownscaleFilter(Size.Relative(2), Size.Relative(1)));
            yield return new TestCaseData("_input.bmp",
                new BoxDownscaleFilter(Size.Relative(1), Size.Relative(2)));

            yield return new TestCaseData("_input.bmp",
                    new CropFilter(Size.Absolute(-1), Size.Absolute(0), Size.Relative(1), Size.Relative(1)));
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Size.Absolute(0), Size.Absolute(-1), Size.Relative(1), Size.Relative(1)));
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Size.Absolute(0), Size.Absolute(0), Size.Relative(0), Size.Relative(1)));
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Size.Absolute(0), Size.Absolute(0), Size.Relative(1), Size.Relative(0)));

            yield return new TestCaseData("_input.bmp",
                new ResizeFilter(Size.Relative(0), Size.Relative(1), InterpolationMode.NearestNeighbor));
            yield return new TestCaseData("_input.bmp",
                new ResizeFilter(Size.Relative(1), Size.Relative(0), InterpolationMode.NearestNeighbor));
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.That(Common.CheckFilter("_input.bmp", expected, filter, tolerance));

        [Test]
        [TestCaseSource("Exceptions")]
        public void TestEx(string expected, IFilter filter) =>
            Assert.Throws<ArgumentException>(() => Common.CheckFilter("_input.bmp", expected, filter));
    }
}
