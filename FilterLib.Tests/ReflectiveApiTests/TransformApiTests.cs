using FilterLib.Filters.Transform;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class TransformApiTests
    {
        [Test]
        public void TestFlipHorizontalParse() =>
            Assert.IsInstanceOf<FlipHorizontalFilter>(ReflectiveApi.ConstructFilterByName("FlipHorizontal"));

        [Test]
        public void TestFlipVerticalParse() =>
            Assert.IsInstanceOf<FlipVerticalFilter>(ReflectiveApi.ConstructFilterByName("FlipVertical"));

        [Test]
        public void TestRotate180Parse() => 
            Assert.IsInstanceOf<Rotate180Filter>(ReflectiveApi.ConstructFilterByName("Rotate180"));

        [Test]
        public void TestRotateLeftParse() => 
            Assert.IsInstanceOf<RotateLeftFilter>(ReflectiveApi.ConstructFilterByName("RotateLeft"));

        [Test]
        public void TestRotateRightParse() => 
            Assert.IsInstanceOf<RotateRightFilter>(ReflectiveApi.ConstructFilterByName("RotateRight"));
    }
}
