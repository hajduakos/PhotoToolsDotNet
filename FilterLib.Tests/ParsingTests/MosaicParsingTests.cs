using FilterLib.Filters.Mosaic;
using FilterLib.Scripting;
using NUnit.Framework;

namespace FilterLib.Tests.ParsingTests
{
    public class MosaicParsingTests
    {
        [Test]
        public void TestBrightnessParse()
        {
            var filters = Parser.Parse(new string[] { "Crystallize" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<CrystallizeFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "Crystallize", "- Size: 30", "- Averaging: 50", "- Seed: 70" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<CrystallizeFilter>(filters[0]);
            CrystallizeFilter f = filters[0] as CrystallizeFilter;
            Assert.AreEqual(30, f.Size);
            Assert.AreEqual(50, f.Averaging);
            Assert.AreEqual(70, f.Seed);
        }
    }
}
