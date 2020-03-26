using FilterLib.Filters.Sharpen;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class SharpenApiTests
    {
        [Test]
        public void TestSharpen() =>
            Assert.IsInstanceOf<SharpenFilter>(ReflectiveApi.ConstructFilterByName("Sharpen"));

        [Test]
        public void TestMeanRemoval() =>
            Assert.IsInstanceOf<MeanRemovalFilter>(ReflectiveApi.ConstructFilterByName("MeanRemoval"));
    }
}
