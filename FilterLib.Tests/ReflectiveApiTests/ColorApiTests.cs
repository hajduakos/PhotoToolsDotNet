﻿using FilterLib.Filters;
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
            Assert.IsInstanceOf<GradientMapFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "GradientMap", "0 (255 0 0), 0.5 (255 255 0), 1 (0 255 0)");
            GradientMapFilter ff = f as GradientMapFilter;
            Assert.AreEqual(new RGB(255, 0, 0), ff.GradientMap.GetColor(0));
            Assert.AreEqual(new RGB(255, 255, 0), ff.GradientMap.GetColor(0.5f));
            Assert.AreEqual(new RGB(0, 255, 0), ff.GradientMap.GetColor(1));
        }

        [Test]
        public void TestGradientMapParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(GradientMapFilter)));

        [Test]
        public void TestGrayscale()
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
        public void TestGrayscaleParCnt() => Assert.AreEqual(3, Common.ParamCount(typeof(GrayscaleFilter)));

        [Test]
        public void TestInvert() => 
            Assert.IsInstanceOf<InvertFilter>(ReflectiveApi.ConstructFilterByName("Invert"));

        [Test]
        public void TestInvertParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(InvertFilter)));

        [Test]
        public void TestOctreeQuantizer()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("OctreeQuantizer");
            Assert.IsInstanceOf<OctreeQuantizerFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "8");
            OctreeQuantizerFilter ff = f as OctreeQuantizerFilter;
            Assert.AreEqual(8, ff.Levels);
        }

        [Test]
        public void TestOctreeQuantizerParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(PosterizeFilter)));

        [Test]
        public void TestOrton()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Orton");
            Assert.IsInstanceOf<OrtonFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Strength", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "10");
            OrtonFilter ff = f as OrtonFilter;
            Assert.AreEqual(50, ff.Strength);
            Assert.AreEqual(10, ff.Radius);
        }

        [Test]
        public void TestOrtonParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(OrtonFilter)));

        [Test]
        public void TestPosterize()
        {
             IFilter f = ReflectiveApi.ConstructFilterByName("Posterize");
            Assert.IsInstanceOf<PosterizeFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "8");
            PosterizeFilter ff = f as PosterizeFilter;
            Assert.AreEqual(8, ff.Levels);
        }

        [Test]
        public void TestPosterizeParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(PosterizeFilter)));

        [Test]
        public void TestSepia() =>
            Assert.IsInstanceOf<SepiaFilter>(ReflectiveApi.ConstructFilterByName("Sepia"));

        [Test]
        public void TestSepiaParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(SepiaFilter)));

        [Test]
        public void TestSolarize() =>
            Assert.IsInstanceOf<SolarizeFilter>(ReflectiveApi.ConstructFilterByName("Solarize"));

        [Test]
        public void TestSolarizeParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(SolarizeFilter)));

        [Test]
        public void TestTreshold()
        {
             IFilter f = ReflectiveApi.ConstructFilterByName("Treshold");
            Assert.IsInstanceOf<TresholdFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Treshold", "127");
            TresholdFilter ff = f as TresholdFilter;
            Assert.AreEqual(127, ff.Treshold);
        }

        [Test]
        public void TestTresholdParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(TresholdFilter)));

        [Test]
        public void TestVintage()
        {
             IFilter f = ReflectiveApi.ConstructFilterByName("Vintage");
            Assert.IsInstanceOf<VintageFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Strength", "80");
            VintageFilter ff = f as VintageFilter;
            Assert.AreEqual(80, ff.Strength);
        }

        [Test]
        public void TestVintageParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(VintageFilter)));
    }
}
