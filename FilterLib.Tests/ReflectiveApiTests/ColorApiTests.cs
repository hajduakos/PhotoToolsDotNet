using FilterLib.Filters;
using FilterLib.Filters.Color;
using FilterLib.Util;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class ColorApiTests
    {
        [Test]
        public void TestGradientMap()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("GradientMap");
            Assert.That(f, Is.InstanceOf<GradientMapFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "GradientMap", "0 (255 0 0), 0.5 (255 255 0), 1 (0 255 0)");
            GradientMapFilter ff = f as GradientMapFilter;
            Assert.That(ff.GradientMap.GetColor(0), Is.EqualTo(new RGB(255, 0, 0)));
            Assert.That(ff.GradientMap.GetColor(0.5f), Is.EqualTo(new RGB(255, 255, 0)));
            Assert.That(ff.GradientMap.GetColor(1), Is.EqualTo(new RGB(0, 255, 0)));
        }

        [Test]
        public void TestGradientMapParCnt() => Assert.That(Common.ParamCount(typeof(GradientMapFilter)), Is.EqualTo(1));

        [Test]
        public void TestGrayscale()
        {
             IFilter f = ReflectiveApi.ConstructFilterByName("Grayscale" );
            Assert.That(f, Is.InstanceOf<GrayscaleFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Red", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Green", "20");
            ReflectiveApi.SetFilterPropertyByName(f, "Blue", "30");
            GrayscaleFilter ff = f as GrayscaleFilter;
            Assert.That(ff.Red, Is.EqualTo(10));
            Assert.That(ff.Green, Is.EqualTo(20));
            Assert.That(ff.Blue, Is.EqualTo(30));
        }

        [Test]
        public void TestGrayscaleParCnt() => Assert.That(Common.ParamCount(typeof(GrayscaleFilter)), Is.EqualTo(3));

        [Test]
        public void TestInvert() => 
            Assert.That(ReflectiveApi.ConstructFilterByName("Invert"), Is.InstanceOf<InvertFilter>());

        [Test]
        public void TestInvertParCnt() => Assert.That(Common.ParamCount(typeof(InvertFilter)), Is.EqualTo(0));

        [Test]
        public void TestOctreeQuantizer()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("OctreeQuantizer");
            Assert.That(f, Is.InstanceOf<OctreeQuantizerFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "8");
            OctreeQuantizerFilter ff = f as OctreeQuantizerFilter;
            Assert.That(ff.Levels, Is.EqualTo(8));
        }

        [Test]
        public void TestOctreeQuantizerParCnt() => Assert.That(Common.ParamCount(typeof(PosterizeFilter)), Is.EqualTo(1));

        [Test]
        public void TestOrton()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Orton");
            Assert.That(f, Is.InstanceOf<OrtonFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Strength", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "10");
            OrtonFilter ff = f as OrtonFilter;
            Assert.That(ff.Strength, Is.EqualTo(50));
            Assert.That(ff.Radius, Is.EqualTo(10));
        }

        [Test]
        public void TestOrtonParCnt() => Assert.That(Common.ParamCount(typeof(OrtonFilter)), Is.EqualTo(2));

        [Test]
        public void TestPosterize()
        {
             IFilter f = ReflectiveApi.ConstructFilterByName("Posterize");
            Assert.That(f, Is.InstanceOf<PosterizeFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "8");
            PosterizeFilter ff = f as PosterizeFilter;
            Assert.That(ff.Levels, Is.EqualTo(8));
        }

        [Test]
        public void TestPosterizeParCnt() => Assert.That(Common.ParamCount(typeof(PosterizeFilter)), Is.EqualTo(1));

        [Test]
        public void TestSepia() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("Sepia"), Is.InstanceOf<SepiaFilter>());

        [Test]
        public void TestSepiaParCnt() => Assert.That(Common.ParamCount(typeof(SepiaFilter)), Is.EqualTo(0));

        [Test]
        public void TestSolarize() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("Solarize"), Is.InstanceOf<SolarizeFilter>());

        [Test]
        public void TestSolarizeParCnt() => Assert.That(Common.ParamCount(typeof(SolarizeFilter)), Is.EqualTo(0));

        [Test]
        public void TestTreshold()
        {
             IFilter f = ReflectiveApi.ConstructFilterByName("Treshold");
            Assert.That(f, Is.InstanceOf<TresholdFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Treshold", "127");
            TresholdFilter ff = f as TresholdFilter;
            Assert.That(ff.Treshold, Is.EqualTo(127));
        }

        [Test]
        public void TestTresholdParCnt() => Assert.That(Common.ParamCount(typeof(TresholdFilter)), Is.EqualTo(1));

        [Test]
        public void TestVintage()
        {
             IFilter f = ReflectiveApi.ConstructFilterByName("Vintage");
            Assert.That(f, Is.InstanceOf<VintageFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Strength", "80");
            VintageFilter ff = f as VintageFilter;
            Assert.That(ff.Strength, Is.EqualTo(80));
        }

        [Test]
        public void TestVintageParCnt() => Assert.That(Common.ParamCount(typeof(VintageFilter)), Is.EqualTo(1));
    }
}
