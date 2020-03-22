using FilterLib.Filters.Color;
using FilterLib.Scripting;
using NUnit.Framework;

namespace FilterLib.Tests.ParsingTests
{
    public class ColorParsingTests
    {
        [Test]
        public void TestInvertParse()
        {
            var filters = Parser.Parse(new string[] { "Invert" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<InvertFilter>(filters[0]);
        }
    }
}
