using FilterLib.Filters;
using FilterLib.Filters.Color;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class ColorApiTests
    {
        [Test]
        public void TestGrayscaleParse()
        {
             IFilter f = ReflectiveApi.ConstructFilterByName("Grayscale" );
            Assert.IsInstanceOf<GrayscaleFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Red", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Green", "20");
            ReflectiveApi.SetFilterPropertyByName(f, "Blue", "30");
            GrayscaleFilter ff = f as GrayscaleFilter;
            Assert.AreEqual(10, ff.Red);
            Assert.AreEqual(20, ff.Green);
            Assert.AreEqual(30, ff.Blue);
        }

        [Test]
        public void TestInvertParse() => 
            Assert.IsInstanceOf<InvertFilter>(ReflectiveApi.ConstructFilterByName("Invert"));

        [Test]
        public void TestPosterizeParse()
        {
             IFilter f = ReflectiveApi.ConstructFilterByName("Posterize");
            Assert.IsInstanceOf<PosterizeFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "8");
            PosterizeFilter ff = f as PosterizeFilter;
            Assert.AreEqual(8, ff.Levels);
        }

        [Test]
        public void TestSepiaParse() =>
            Assert.IsInstanceOf<SepiaFilter>(ReflectiveApi.ConstructFilterByName("Sepia"));

        [Test]
        public void TestTresholdParse()
        {
             IFilter f = ReflectiveApi.ConstructFilterByName("Treshold");
            Assert.IsInstanceOf<TresholdFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Treshold", "127");
            TresholdFilter ff = f as TresholdFilter;
            Assert.AreEqual(127, ff.Treshold);
        }

        [Test]
        public void TestVintageParse()
        {
             IFilter f = ReflectiveApi.ConstructFilterByName("Vintage");
            Assert.IsInstanceOf<VintageFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Strength", "80");
            VintageFilter ff = f as VintageFilter;
            Assert.AreEqual(80, ff.Strength);
        }
    }
}
