using FilterLib.Filters.Artistic;
using FilterLib.Scripting;
using NUnit.Framework;

namespace FilterLib.Tests.ParsingTests
{
    public class ArtisticParsingTests
    {

        [Test]
        public void TestAdaptiveTresholdParse()
        {
            var filters = Parser.Parse(new string[] { "AdaptiveTreshold" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<AdaptiveTresholdFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "AdaptiveTreshold", "- SquareSize: 8" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<AdaptiveTresholdFilter>(filters[0]);
            AdaptiveTresholdFilter f = filters[0] as AdaptiveTresholdFilter;
            Assert.AreEqual(8, f.SquareSize);
        }
    }
}
