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
    }
}
