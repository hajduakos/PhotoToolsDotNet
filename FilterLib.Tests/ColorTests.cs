using NUnit.Framework;
using FilterLib.Filters.Color;

namespace FilterLib.Tests
{
    public class ColorTests
    {
        [Test]
        public void TestInvert()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Invert.bmp", new InvertFilter(), 0));
        }
    }
}
