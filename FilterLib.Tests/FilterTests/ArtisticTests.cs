using FilterLib.Filters.Artistic;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class ArtisticTests
    {
        [Test]
        public void TestAdaptiveTreshold()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "AdaptiveTreshold_1.bmp", new AdaptiveTresholdFilter(1), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "AdaptiveTreshold_4.bmp", new AdaptiveTresholdFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "AdaptiveTreshold_8.bmp", new AdaptiveTresholdFilter(8), 1));
        }
    }
}
