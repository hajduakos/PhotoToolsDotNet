﻿using FilterLib.Filters;
using FilterLib.Filters.Generate;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class GenerateApiTests
    {
        [Test]
        public void TestLinearGradient()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("LinearGradient");
            Assert.IsInstanceOf<LinearGradientFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "StartX", "12px");
            ReflectiveApi.SetFilterPropertyByName(f, "StartY", "23%");
            ReflectiveApi.SetFilterPropertyByName(f, "EndX", "45px");
            ReflectiveApi.SetFilterPropertyByName(f, "EndY", "56%");
            LinearGradientFilter ff = f as LinearGradientFilter;
            Assert.AreEqual(12, ff.StartX.ToAbsolute(200));
            Assert.AreEqual(46, ff.StartY.ToAbsolute(200));
            Assert.AreEqual(45, ff.EndX.ToAbsolute(200));
            Assert.AreEqual(112, ff.EndY.ToAbsolute(200));
        }

        [Test]
        public void TestLinearGradientParCnt() => Assert.AreEqual(4, Common.ParamCount(typeof(LinearGradientFilter)));

        [Test]
        public void TestMarble()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Marble");
            Assert.IsInstanceOf<MarbleFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "HorizontalLines", "1");
            ReflectiveApi.SetFilterPropertyByName(f, "VerticalLines", "2");
            ReflectiveApi.SetFilterPropertyByName(f, "Twist", "3");
            ReflectiveApi.SetFilterPropertyByName(f, "Iterations", "4");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "5");
            MarbleFilter ff = f as MarbleFilter;
            Assert.AreEqual(1, ff.HorizontalLines);
            Assert.AreEqual(2, ff.VerticalLines);
            Assert.AreEqual(3, ff.Twist);
            Assert.AreEqual(4, ff.Iterations);
            Assert.AreEqual(5, ff.Seed);
        }

        [Test]
        public void TestMarbleParCnt() => Assert.AreEqual(5, Common.ParamCount(typeof(MarbleFilter)));

        [Test]
        public void TestRadialGradient()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("RadialGradient");
            Assert.IsInstanceOf<RadialGradientFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "CenterX", "12px");
            ReflectiveApi.SetFilterPropertyByName(f, "CenterY", "23%");
            ReflectiveApi.SetFilterPropertyByName(f, "InnerRadius", "45px");
            ReflectiveApi.SetFilterPropertyByName(f, "OuterRadius", "56%");
            RadialGradientFilter ff = f as RadialGradientFilter;
            Assert.AreEqual(12, ff.CenterX.ToAbsolute(200));
            Assert.AreEqual(46, ff.CenterY.ToAbsolute(200));
            Assert.AreEqual(45, ff.InnerRadius.ToAbsolute(200));
            Assert.AreEqual(112, ff.OuterRadius.ToAbsolute(200));
        }

        [Test]
        public void TestRadialGradientParCnt() => Assert.AreEqual(4, Common.ParamCount(typeof(RadialGradientFilter)));

        [Test]
        public void TestTurbulence()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Turbulence");
            Assert.IsInstanceOf<TurbulenceFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Iterations", "4");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "5");
            TurbulenceFilter ff = f as TurbulenceFilter;
            Assert.AreEqual(4, ff.Iterations);
            Assert.AreEqual(5, ff.Seed);
        }

        [Test]
        public void TestTurbulenceParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(TurbulenceFilter)));

        [Test]
        public void TestWoodRings()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("WoodRings");
            Assert.IsInstanceOf<WoodRingsFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Rings", "1");
            ReflectiveApi.SetFilterPropertyByName(f, "Twist", "3");
            ReflectiveApi.SetFilterPropertyByName(f, "Iterations", "4");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "5");
            WoodRingsFilter ff = f as WoodRingsFilter;
            Assert.AreEqual(1, ff.Rings);
            Assert.AreEqual(3, ff.Twist);
            Assert.AreEqual(4, ff.Iterations);
            Assert.AreEqual(5, ff.Seed);
        }

        [Test]
        public void TestWoodRingsParCnt() => Assert.AreEqual(4, Common.ParamCount(typeof(WoodRingsFilter)));
    }
}
