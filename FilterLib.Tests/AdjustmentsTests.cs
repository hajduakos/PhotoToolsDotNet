using NUnit.Framework;
using FilterLib.Adjustments;

namespace FilterLib.Tests
{
    public class AdjustmentsTests
    {
        [Test]
        public void TestBrightness()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Brightness_0.bmp", new BrightnessFilter(0), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Brightness_50.bmp", new BrightnessFilter(50), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Brightness_-100.bmp", new BrightnessFilter(-100), 0));
        }
    }
}
