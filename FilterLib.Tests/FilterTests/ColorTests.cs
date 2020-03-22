using NUnit.Framework;
using FilterLib.Filters.Color;

namespace FilterLib.Tests.FilterTests
{
    public class ColorTests
    {
        [Test]
        public void TestGrayscale()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Grayscale_100_0_0.bmp", new GrayscaleFilter(100, 0, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Grayscale_0_100_0.bmp", new GrayscaleFilter(0, 100, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Grayscale_0_0_100.bmp", new GrayscaleFilter(0, 0, 100), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Grayscale_30_59_11.bmp", new GrayscaleFilter(30, 59, 11), 1));
        }

        [Test]
        public void TestInvert()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Invert.bmp", new InvertFilter(), 0));
        }

        [Test]
        public void TestPosterize()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Posterize_2.bmp", new PosterizeFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Posterize_4.bmp", new PosterizeFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Posterize_8.bmp", new PosterizeFilter(8), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new PosterizeFilter(256), 1));
        }
    }
}
