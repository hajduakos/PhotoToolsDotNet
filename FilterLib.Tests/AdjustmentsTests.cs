using NUnit.Framework;
using FilterLib.Adjustments;

namespace FilterLib.Tests
{
    public class AdjustmentsTests
    {
        readonly string path = TestContext.CurrentContext.TestDirectory + "\\TestImages\\";
        [Test]
        public void TestBrightness()
        {
            Assert.IsTrue(Common.CheckFilter(path + "_input.bmp", path + "Brightness_0.bmp", new BrightnessFilter(0), 0));
            Assert.IsTrue(Common.CheckFilter(path + "_input.bmp", path + "Brightness_50.bmp", new BrightnessFilter(50), 0));
            Assert.IsTrue(Common.CheckFilter(path + "_input.bmp", path + "Brightness_-100.bmp", new BrightnessFilter(-100), 0));
        }
    }
}
