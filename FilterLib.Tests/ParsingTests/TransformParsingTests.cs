using FilterLib.Filters.Transform;
using FilterLib.Scripting;
using NUnit.Framework;

namespace FilterLib.Tests.ParsingTests
{
    public class TransformParsingTests
    {
        [Test]
        public void TestFlipHorizontalParse()
        {
            var filters = Parser.Parse(new string[] { "FlipHorizontal" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<FlipHorizontalFilter>(filters[0]);
        }

        [Test]
        public void TestFlipVerticalParse()
        {
            var filters = Parser.Parse(new string[] { "FlipVertical" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<FlipVerticalFilter>(filters[0]);
        }

        [Test]
        public void TestRotate180Parse()
        {
            var filters = Parser.Parse(new string[] { "Rotate180" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<Rotate180Filter>(filters[0]);
        }

        [Test]
        public void TestRotateLeftParse()
        {
            var filters = Parser.Parse(new string[] { "RotateLeft" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<RotateLeftFilter>(filters[0]);
        }

        [Test]
        public void TestRotateRightParse()
        {
            var filters = Parser.Parse(new string[] { "RotateRight" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<RotateRightFilter>(filters[0]);
        }
    }
}
