using NUnit.Framework;
using FilterLib.Color;

namespace FilterLib.Tests
{
    public class ColorTests
    {
        readonly string path = TestContext.CurrentContext.TestDirectory + "/TestImages/";

        [Test]
        public void TestInvert()
        {
            Assert.IsTrue(Common.CheckFilter(path + "_input.bmp", path + "Invert.bmp", new InvertFilter(), 0));
        }
    }
}
