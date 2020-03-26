using FilterLib.Filters.Sharpen;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class EdgesTests
    {
        [Test]
        public void TestEdgeDetection() =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "EdgeDetection.bmp", new EdgeDetectionFilter(), 1));

        [Test]
        public void TestEmboss() =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Emboss.bmp", new EmbossFilter(), 1));

        [Test]
        public void TestSobel() =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Sobel.bmp", new SobelFilter(), 1));
    }
}
