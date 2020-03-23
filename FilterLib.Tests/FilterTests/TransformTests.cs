using FilterLib.Filters.Transform;
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
