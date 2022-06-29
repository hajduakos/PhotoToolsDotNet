using FilterLib.Filters.Edges;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class EdgesApiTests
    {
        [Test]
        public void TestEdgeDetection() =>
            Assert.IsInstanceOf<EdgeDetectionFilter>(ReflectiveApi.ConstructFilterByName("EdgeDetection"));

        [Test]
        public void TestEdgeDetectionParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(EdgeDetectionFilter)));

        [Test]
        public void TestEmboss() =>
            Assert.IsInstanceOf<EmbossFilter>(ReflectiveApi.ConstructFilterByName("Emboss"));

        [Test]
        public void TestEmbossParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(EmbossFilter)));

        [Test]
        public void TestPrewitt() =>
            Assert.IsInstanceOf<PrewittFilter>(ReflectiveApi.ConstructFilterByName("Prewitt"));

        [Test]
        public void TestPrewittParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(PrewittFilter)));

        [Test]
        public void TestSobel() =>
            Assert.IsInstanceOf<SobelFilter>(ReflectiveApi.ConstructFilterByName("Sobel"));

        [Test]
        public void TestSobelParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(SobelFilter)));
    }
}
