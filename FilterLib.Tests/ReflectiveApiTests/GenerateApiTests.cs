using FilterLib.Filters;
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
            Assert.That(f, Is.InstanceOf<LinearGradientFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "StartX", "12px");
            ReflectiveApi.SetFilterPropertyByName(f, "StartY", "23%");
            ReflectiveApi.SetFilterPropertyByName(f, "EndX", "45px");
            ReflectiveApi.SetFilterPropertyByName(f, "EndY", "56%");
            LinearGradientFilter ff = f as LinearGradientFilter;
            Assert.That(ff.StartX.ToAbsolute(200), Is.EqualTo(12));
            Assert.That(ff.StartY.ToAbsolute(200), Is.EqualTo(46));
            Assert.That(ff.EndX.ToAbsolute(200), Is.EqualTo(45));
            Assert.That(ff.EndY.ToAbsolute(200), Is.EqualTo(112));
        }

        [Test]
        public void TestLinearGradientParCnt() => Assert.That(Common.ParamCount(typeof(LinearGradientFilter)), Is.EqualTo(4));

        [Test]
        public void TestMarble()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Marble");
            Assert.That(f, Is.InstanceOf<MarbleFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "HorizontalLines", "1");
            ReflectiveApi.SetFilterPropertyByName(f, "VerticalLines", "2");
            ReflectiveApi.SetFilterPropertyByName(f, "Twist", "3");
            ReflectiveApi.SetFilterPropertyByName(f, "Iterations", "4");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "5");
            MarbleFilter ff = f as MarbleFilter;
            Assert.That(ff.HorizontalLines, Is.EqualTo(1));
            Assert.That(ff.VerticalLines, Is.EqualTo(2));
            Assert.That(ff.Twist, Is.EqualTo(3));
            Assert.That(ff.Iterations, Is.EqualTo(4));
            Assert.That(ff.Seed, Is.EqualTo(5));
        }

        [Test]
        public void TestMarbleParCnt() => Assert.That(Common.ParamCount(typeof(MarbleFilter)), Is.EqualTo(5));

        [Test]
        public void TestRadialGradient()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("RadialGradient");
            Assert.That(f, Is.InstanceOf<RadialGradientFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "CenterX", "12px");
            ReflectiveApi.SetFilterPropertyByName(f, "CenterY", "23%");
            ReflectiveApi.SetFilterPropertyByName(f, "InnerRadius", "45px");
            ReflectiveApi.SetFilterPropertyByName(f, "OuterRadius", "56%");
            RadialGradientFilter ff = f as RadialGradientFilter;
            Assert.That(ff.CenterX.ToAbsolute(200), Is.EqualTo(12));
            Assert.That(ff.CenterY.ToAbsolute(200), Is.EqualTo(46));
            Assert.That(ff.InnerRadius.ToAbsolute(200), Is.EqualTo(45));
            Assert.That(ff.OuterRadius.ToAbsolute(200), Is.EqualTo(112));
        }

        [Test]
        public void TestRadialGradientParCnt() => Assert.That(Common.ParamCount(typeof(RadialGradientFilter)), Is.EqualTo(4));

        [Test]
        public void TestTurbulence()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Turbulence");
            Assert.That(f, Is.InstanceOf<TurbulenceFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Iterations", "4");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "5");
            TurbulenceFilter ff = f as TurbulenceFilter;
            Assert.That(ff.Iterations, Is.EqualTo(4));
            Assert.That(ff.Seed, Is.EqualTo(5));
        }

        [Test]
        public void TestTurbulenceParCnt() => Assert.That(Common.ParamCount(typeof(TurbulenceFilter)), Is.EqualTo(2));

        [Test]
        public void TestWoodRings()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("WoodRings");
            Assert.That(f, Is.InstanceOf<WoodRingsFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Rings", "1");
            ReflectiveApi.SetFilterPropertyByName(f, "Twist", "3");
            ReflectiveApi.SetFilterPropertyByName(f, "Iterations", "4");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "5");
            WoodRingsFilter ff = f as WoodRingsFilter;
            Assert.That(ff.Rings, Is.EqualTo(1));
            Assert.That(ff.Twist, Is.EqualTo(3));
            Assert.That(ff.Iterations, Is.EqualTo(4));
            Assert.That(ff.Seed, Is.EqualTo(5));
        }

        [Test]
        public void TestWoodRingsParCnt() => Assert.That(Common.ParamCount(typeof(WoodRingsFilter)), Is.EqualTo(4));
    }
}
