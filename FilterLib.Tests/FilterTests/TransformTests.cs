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
    }
}
