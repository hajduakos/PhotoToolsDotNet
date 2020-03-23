using FilterLib.Filters.Mosaic;
using FilterLib.Scripting;
using NUnit.Framework;

namespace FilterLib.Tests.ParsingTests
{
    public class MosaicParsingTests
    {
        [Test]
        public void TestCrystallizeParse()
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

        [Test]
        public void TestLegoParse()
        {
            var filters = Parser.Parse(new string[] { "Lego" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<LegoFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "Lego", "- Size: 30" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<LegoFilter>(filters[0]);
            LegoFilter f = filters[0] as LegoFilter;
            Assert.AreEqual(30, f.Size);
        }
    }
}
