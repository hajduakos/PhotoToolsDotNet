using FilterLib.Filters.Blur;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class BlurTests
    {
        [Test]
        public void TestEdgeDetection() =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Blur.bmp", new BlurFilter(), 1));
    }
}
