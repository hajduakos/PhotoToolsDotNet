using FilterLib.Filters.Edges;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class EdgesApiTests
    {
        [Test]
        public void TestEdgeDetection() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("EdgeDetection"), Is.InstanceOf<EdgeDetectionFilter>());

        [Test]
        public void TestEdgeDetectionParCnt() => Assert.That(Common.ParamCount(typeof(EdgeDetectionFilter)), Is.EqualTo(0));

        [Test]
        public void TestEmboss() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("Emboss"), Is.InstanceOf<EmbossFilter>());

        [Test]
        public void TestEmbossParCnt() => Assert.That(Common.ParamCount(typeof(EmbossFilter)), Is.EqualTo(0));

        [Test]
        public void TestPrewitt() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("Prewitt"), Is.InstanceOf<PrewittFilter>());

        [Test]
        public void TestPrewittParCnt() => Assert.That(Common.ParamCount(typeof(PrewittFilter)), Is.EqualTo(0));

        [Test]
        public void TestSobel() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("Sobel"), Is.InstanceOf<SobelFilter>());

        [Test]
        public void TestSobelParCnt() => Assert.That(Common.ParamCount(typeof(SobelFilter)), Is.EqualTo(0));

        [Test]
        public void TestScharr() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("Scharr"), Is.InstanceOf<ScharrFilter>());

        [Test]
        public void TestScharrParCnt() => Assert.That(Common.ParamCount(typeof(ScharrFilter)), Is.EqualTo(0));
    }
}
