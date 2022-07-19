using FilterLib.Filters;
using FilterLib.Filters.Noise;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class NoiseApiTests
    {

        [Test]
        public void TestAddNoise()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("AddNoise");
            Assert.IsInstanceOf<AddNoiseFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Intensity", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Strength", "60");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "70");
            ReflectiveApi.SetFilterPropertyByName(f, "Type", "Monochrome");
            AddNoiseFilter ff = f as AddNoiseFilter;
            Assert.AreEqual(50, ff.Intensity);
            Assert.AreEqual(60, ff.Strength);
            Assert.AreEqual(70, ff.Seed);
            Assert.AreEqual(AddNoiseFilter.NoiseType.Monochrome, ff.Type);
        }

        [Test]
        public void TestAddNoiseParCnt() => Assert.AreEqual(4, Common.ParamCount(typeof(AddNoiseFilter)));


        [Test]
        public void TestMedian()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Median");
            Assert.IsInstanceOf<MedianFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Strength", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "3");
            MedianFilter ff = f as MedianFilter;
            Assert.AreEqual(50, ff.Strength);
            Assert.AreEqual(3, ff.Radius);
        }

        [Test]
        public void TestMedianParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(MedianFilter)));
    }
}
