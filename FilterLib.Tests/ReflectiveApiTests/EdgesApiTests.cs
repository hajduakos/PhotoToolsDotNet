using FilterLib.Filters.Sharpen;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class EdgesApiTests
    {
        [Test]
        public void TestEdgeDetection() =>
            Assert.IsInstanceOf<EdgeDetectionFilter>(ReflectiveApi.ConstructFilterByName("EdgeDetection"));

        [Test]
        public void TestEmboss() =>
            Assert.IsInstanceOf<EmbossFilter>(ReflectiveApi.ConstructFilterByName("Emboss"));

        [Test]
        public void TestPrewitt() =>
            Assert.IsInstanceOf<SobelFilter>(ReflectiveApi.ConstructFilterByName("Prewitt"));

        [Test]
        public void TestSobel() =>
            Assert.IsInstanceOf<SobelFilter>(ReflectiveApi.ConstructFilterByName("Sobel"));
    }
}
