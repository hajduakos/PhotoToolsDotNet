﻿using FilterLib.Filters.Transform;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class TransformTests
    {
        [Test]
        public void TestFlipHorizontal() =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "FlipHorizontal.bmp", new FlipHorizontalFilter(), 0));

        [Test]
        public void TestFlipVertical() =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "FlipVertical.bmp", new FlipVerticalFilter(), 0));

        [Test]
        public void TestResize()
        {
            const System.Drawing.Drawing2D.InterpolationMode itp = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp",
                new ResizeFilter(Util.Size.Relative(1), Util.Size.Relative(1), itp), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Resize_200pct_30px_HQB.bmp",
                new ResizeFilter(Util.Size.Relative(2), Util.Size.Absolute(30), itp), 2));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Resize_25pct_50pct_HQB.bmp",
                new ResizeFilter(Util.Size.Relative(.25f), Util.Size.Relative(.5f), itp), 2));
        }

        [Test]
        public void TestRotate180() =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Rotate180.bmp", new Rotate180Filter(), 0));

        [Test]
        public void TestRotateLeft() =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "RotateLeft.bmp", new RotateLeftFilter(), 0));

        [Test]
        public void TestRotateRight() =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "RotateRight.bmp", new RotateRightFilter(), 0));
    }
}
