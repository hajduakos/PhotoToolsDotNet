using FilterLib.Filters.Transform;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class TransformApiTests
    {
        [Test]
        public void TestFlipHorizontal() =>
            Assert.IsInstanceOf<FlipHorizontalFilter>(ReflectiveApi.ConstructFilterByName("FlipHorizontal"));

        [Test]
        public void TestFlipVertical() =>
            Assert.IsInstanceOf<FlipVerticalFilter>(ReflectiveApi.ConstructFilterByName("FlipVertical"));

        [Test]
        public void TestRotate180() => 
            Assert.IsInstanceOf<Rotate180Filter>(ReflectiveApi.ConstructFilterByName("Rotate180"));

        [Test]
        public void TestRotateLeft() => 
            Assert.IsInstanceOf<RotateLeftFilter>(ReflectiveApi.ConstructFilterByName("RotateLeft"));

        [Test]
        public void TestRotateRight() => 
            Assert.IsInstanceOf<RotateRightFilter>(ReflectiveApi.ConstructFilterByName("RotateRight"));
    }
}
