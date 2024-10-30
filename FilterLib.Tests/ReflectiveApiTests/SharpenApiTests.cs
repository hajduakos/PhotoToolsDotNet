using FilterLib.Filters.Sharpen;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class SharpenApiTests
    {
        [Test]
        public void TestSharpen() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("Sharpen"), Is.InstanceOf<SharpenFilter>());

        [Test]
        public void TestSharpenParCnt() => Assert.That(Common.ParamCount(typeof(SharpenFilter)), Is.EqualTo(0));

        [Test]
        public void TestMeanRemoval() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("MeanRemoval"), Is.InstanceOf<MeanRemovalFilter>());

        [Test]
        public void TestMeanRemovalParCnt() => Assert.That(Common.ParamCount(typeof(MeanRemovalFilter)), Is.EqualTo(0));
    }
}
