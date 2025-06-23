using FilterLib.Filters;
using FilterLib.Filters.Dither;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class DitherApiTests
    {
        [Test]
        public void TestBayerDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("BayerDither");
            Assert.That(f, Is.InstanceOf<BayerDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Size", "8");
            BayerDitherFilter ff = f as BayerDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
            Assert.That(ff.Size, Is.EqualTo(8));
        }

        [Test]
        public void TestBayerDitherParCnt() => Assert.That(Common.ParamCount(typeof(BayerDitherFilter)), Is.EqualTo(2));
        [Test]
        public void TestClusterDotDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("ClusterDotDither");
            Assert.That(f, Is.InstanceOf<ClusterDotDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            ClusterDotDitherFilter ff = f as ClusterDotDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
        }

        [Test]
        public void TestClusterDotDitherParCnt() => Assert.That(Common.ParamCount(typeof(ClusterDotDitherFilter)), Is.EqualTo(1));

        [Test]
        public void TestAtkinsonDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("AtkinsonDither");
            Assert.That(f, Is.InstanceOf<AtkinsonDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            AtkinsonDitherFilter ff = f as AtkinsonDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
        }

        [Test]
        public void TestAtkinsonDitherParCnt() => Assert.That(Common.ParamCount(typeof(AtkinsonDitherFilter)), Is.EqualTo(1));

        [Test]
        public void TestBurkesDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("BurkesDither");
            Assert.That(f, Is.InstanceOf<BurkesDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            BurkesDitherFilter ff = f as BurkesDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
        }

        [Test]
        public void TestBurkesDitherParCnt() => Assert.That(Common.ParamCount(typeof(BurkesDitherFilter)), Is.EqualTo(1));

        [Test]
        public void TestFanDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("FanDither");
            Assert.That(f, Is.InstanceOf<FanDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            FanDitherFilter ff = f as FanDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
        }

        [Test]
        public void TestFanDitherParCnt() => Assert.That(Common.ParamCount(typeof(FanDitherFilter)), Is.EqualTo(1));

        [Test]
        public void TestFilterLiteDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("FilterLiteDither");
            Assert.That(f, Is.InstanceOf<FilterLiteDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            FilterLiteDitherFilter ff = f as FilterLiteDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
        }

        [Test]
        public void TestFilterLiteDitherParCnt() => Assert.That(Common.ParamCount(typeof(FilterLiteDitherFilter)), Is.EqualTo(1));

        [Test]
        public void TestShiauFanDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("ShiauFanDither");
            Assert.That(f, Is.InstanceOf<ShiauFanDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            ShiauFanDitherFilter ff = f as ShiauFanDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
        }

        [Test]
        public void TestShiauFanDitherParCnt() => Assert.That(Common.ParamCount(typeof(ShiauFanDitherFilter)), Is.EqualTo(1));

        [Test]
        public void TestFloydSteinbergDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("FloydSteinbergDither");
            Assert.That(f, Is.InstanceOf<FloydSteinbergDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            FloydSteinbergDitherFilter ff = f as FloydSteinbergDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
        }

        [Test]
        public void TestFloydSteinbergDitherParCnt() => Assert.That(Common.ParamCount(typeof(FloydSteinbergDitherFilter)), Is.EqualTo(1));

        [Test]
        public void TestJarvisJudiceNinkeDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("JarvisJudiceNinkeDither");
            Assert.That(f, Is.InstanceOf<JarvisJudiceNinkeDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            JarvisJudiceNinkeDitherFilter ff = f as JarvisJudiceNinkeDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
        }

        [Test]
        public void TestJarvisJudiceNinkeDitherParCnt() => Assert.That(Common.ParamCount(typeof(JarvisJudiceNinkeDitherFilter)), Is.EqualTo(1));

        [Test]
        public void TestRandomDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("RandomDither");
            Assert.That(f, Is.InstanceOf<RandomDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "123");
            RandomDitherFilter ff = f as RandomDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
            Assert.That(ff.Seed, Is.EqualTo(123));
        }

        [Test]
        public void TestRandomDitherParCnt() => Assert.That(Common.ParamCount(typeof(RandomDitherFilter)), Is.EqualTo(2));

        [Test]
        public void TestSierraDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("SierraDither");
            Assert.That(f, Is.InstanceOf<SierraDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            SierraDitherFilter ff = f as SierraDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
        }

        [Test]
        public void TestSierraDitherParCnt() => Assert.That(Common.ParamCount(typeof(SierraDitherFilter)), Is.EqualTo(1));

        [Test]
        public void TestSierraDitherTwoRow()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("SierraDitherTwoRow");
            Assert.That(f, Is.InstanceOf<SierraDitherTwoRowFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            SierraDitherTwoRowFilter ff = f as SierraDitherTwoRowFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
        }

        [Test]
        public void TestSierraDitherTwoRowParCnt() => Assert.That(Common.ParamCount(typeof(SierraDitherTwoRowFilter)), Is.EqualTo(1));

        [Test]
        public void TestStuckiDither()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("StuckiDither");
            Assert.That(f, Is.InstanceOf<StuckiDitherFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Levels", "50");
            StuckiDitherFilter ff = f as StuckiDitherFilter;
            Assert.That(ff.Levels, Is.EqualTo(50));
        }

        [Test]
        public void TestStuckiDitherParCnt() => Assert.That(Common.ParamCount(typeof(StuckiDitherFilter)), Is.EqualTo(1));
    }
}
