using FilterLib.Filters.Blur;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class BlurApiTests
    {

        [Test]
        public void TestEdgeDetection() =>
            Assert.IsInstanceOf<BlurFilter>(ReflectiveApi.ConstructFilterByName("Blur"));
    }
}
