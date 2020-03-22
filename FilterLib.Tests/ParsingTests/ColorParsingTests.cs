using FilterLib.Filters.Color;
using FilterLib.Scripting;
using NUnit.Framework;

namespace FilterLib.Tests.ParsingTests
{
    public class ColorParsingTests
    {
        [Test]
        public void TestGrayscaleParse()
        {
            var filters = Parser.Parse(new string[] { "Grayscale" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<GrayscaleFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "Grayscale", "- Red: 10", "- Green: 20", "- Blue: 30" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<GrayscaleFilter>(filters[0]);
            GrayscaleFilter f = filters[0] as GrayscaleFilter;
            Assert.AreEqual(10, f.Red);
            Assert.AreEqual(20, f.Green);
            Assert.AreEqual(30, f.Blue);
        }

        [Test]
        public void TestInvertParse()
        {
            var filters = Parser.Parse(new string[] { "Invert" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<InvertFilter>(filters[0]);
        }

        [Test]
        public void TestPosterizeParse()
        {
            var filters = Parser.Parse(new string[] { "Posterize" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<PosterizeFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "Posterize", "- Levels: 8" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<PosterizeFilter>(filters[0]);
            PosterizeFilter f = filters[0] as PosterizeFilter;
            Assert.AreEqual(8, f.Levels);
        }
    }
}
