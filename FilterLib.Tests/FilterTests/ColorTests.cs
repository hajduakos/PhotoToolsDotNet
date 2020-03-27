using NUnit.Framework;
using FilterLib.Filters.Color;
using FilterLib.Util;

namespace FilterLib.Tests.FilterTests
{
    public class ColorTests
    {
        [Test]
        public void TestGradientMap()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "GradientMap_r_y_g.bmp", new GradientMapFilter(
                new Gradient(new RGB(255, 0, 0), new RGB(255, 255, 0), new RGB(0, 255, 0))), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "GradientMap_b_r_y.bmp", new GradientMapFilter(
                new Gradient(new RGB(0, 0, 255), new RGB(255, 0, 0), new RGB(255, 255, 0))), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "GradientMap_rainbow.bmp", new GradientMapFilter(
                new Gradient(
                    new RGB[]{
                        new RGB(255,0,0),
                        new RGB(255,0,255),
                        new RGB(0,0,255),
                        new RGB(0,255,255),
                        new RGB(0,255,0),
                        new RGB(255,255,0),
                        new RGB(255,0,0)},
                    new float[] { 1, 2, 3, 4, 5, 6, 7 })), 1));
        }

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
        public void TestOrton()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new OrtonFilter(0, 3), 0));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Orton_80_5.bmp", new OrtonFilter(80, 5), 1));
        }

        [Test]
        public void TestPosterize()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Posterize_2.bmp", new PosterizeFilter(2), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Posterize_4.bmp", new PosterizeFilter(4), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Posterize_8.bmp", new PosterizeFilter(8), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new PosterizeFilter(256), 1));
        }

        [Test]
        public void TestSepia()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Sepia.bmp", new SepiaFilter(), 1));
        }

        [Test]
        public void TestTreshold()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Treshold_63.bmp", new TresholdFilter(63), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Treshold_127.bmp", new TresholdFilter(127), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Treshold_191.bmp", new TresholdFilter(191), 1));
        }

        [Test]
        public void TestVintage()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Vintage_80.bmp", new VintageFilter(80), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new VintageFilter(0), 1));
        }
    }
}
