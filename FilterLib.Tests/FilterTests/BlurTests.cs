using FilterLib.Filters.Blur;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class BlurTests
    {
        [Test]
        public void TestBlur() =>
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Blur.bmp", new BlurFilter(), 1));

        [Test]
        public void TestBoxBlur()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new BoxBlurFilter(0, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "BoxBlur_5_0.bmp", new BoxBlurFilter(5, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "BoxBlur_0_5.bmp", new BoxBlurFilter(0, 5), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "BoxBlur_10_20.bmp", new BoxBlurFilter(10, 20), 1));
        }

        [Test]
        public void TestGaussianBlur()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new GaussianBlurFilter(0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "GaussianBlur_5.bmp", new GaussianBlurFilter(5), 1));
        }

        [Test]
        public void TestMotionBlur()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new MotionBlurFilter(0, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "MotionBlur_10_30.bmp", new MotionBlurFilter(10, 30), 1));
        }
    }
}
