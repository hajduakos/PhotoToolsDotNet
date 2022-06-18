using FilterLib.Filters;
using FilterLib.Filters.Transform;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using InterpolationMode = System.Drawing.Drawing2D.InterpolationMode;

namespace FilterLib.Tests.FilterTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class TransformTests
    {

        private const InterpolationMode itp = InterpolationMode.NearestNeighbor;

        internal static IEnumerable<TestCaseData> Data()
        {
            string suffix = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "" : "_l";

            yield return new TestCaseData("_input.bmp",
                new CropFilter(Util.Size.Absolute(0), Util.Size.Absolute(0), Util.Size.Relative(1), Util.Size.Relative(1)), 1);
            yield return new TestCaseData("Crop_25pct_45px_50pct_40px.bmp",
                new CropFilter(Util.Size.Relative(.25f), Util.Size.Absolute(45), Util.Size.Relative(.5f), Util.Size.Absolute(40)), 1);

            yield return new TestCaseData("FlipHorizontal.bmp", new FlipHorizontalFilter(), 0);

            yield return new TestCaseData("FlipVertical.bmp", new FlipVerticalFilter(), 0);

            yield return new TestCaseData("_input.bmp",
                new ResizeFilter(Util.Size.Relative(1), Util.Size.Relative(1), itp), 1);
            yield return new TestCaseData($"Resize_200pct_30px_NN{suffix}.bmp",
                new ResizeFilter(Util.Size.Relative(2), Util.Size.Absolute(30), itp), 1);
            yield return new TestCaseData($"Resize_25pct_50pct_NN{suffix}.bmp",
                new ResizeFilter(Util.Size.Relative(.25f), Util.Size.Relative(.5f), itp), 1);

            yield return new TestCaseData("_input.bmp", new RotateFilter(0, false), 1);
            yield return new TestCaseData("_input.bmp", new RotateFilter(0, true), 1);
            yield return new TestCaseData($"Rotate_30_false{suffix}.bmp", new RotateFilter(30, false), 1);
            yield return new TestCaseData($"Rotate_30_true{suffix}.bmp", new RotateFilter(30, true), 1);

            yield return new TestCaseData("Rotate180.bmp", new Rotate180Filter(), 0);

            yield return new TestCaseData("RotateLeft.bmp", new RotateLeftFilter(), 0);

            yield return new TestCaseData("RotateRight.bmp", new RotateRightFilter(), 0);
        }

        internal static IEnumerable<TestCaseData> Exceptions()
        {
            yield return new TestCaseData("_input.bmp",
                    new CropFilter(Util.Size.Absolute(-1), Util.Size.Absolute(0), Util.Size.Relative(1), Util.Size.Relative(1)), 1);
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Util.Size.Absolute(0), Util.Size.Absolute(-1), Util.Size.Relative(1), Util.Size.Relative(1)), 1);
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Util.Size.Absolute(0), Util.Size.Absolute(0), Util.Size.Relative(0), Util.Size.Relative(1)), 1);
            yield return new TestCaseData("_input.bmp",
                new CropFilter(Util.Size.Absolute(0), Util.Size.Absolute(0), Util.Size.Relative(1), Util.Size.Relative(0)), 1);

            yield return new TestCaseData("_input.bmp",
                new ResizeFilter(Util.Size.Relative(0), Util.Size.Relative(1), itp), 1);
            yield return new TestCaseData("_input.bmp",
                new ResizeFilter(Util.Size.Relative(1), Util.Size.Relative(0), itp), 1);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IFilter filter, int tolerance) =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", expected, filter, tolerance));

        [Test]
        [TestCaseSource("Exceptions")]
        public void TestEx(string expected, IFilter filter, int tolerance) =>
            Assert.Throws<ArgumentException>(() => Common.CheckFilter("_input.bmp", expected, filter, tolerance));
    }
}
