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
            Assert.That(f, Is.InstanceOf<AddNoiseFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Intensity", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Strength", "60");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "70");
            ReflectiveApi.SetFilterPropertyByName(f, "Type", "Monochrome");
            AddNoiseFilter ff = f as AddNoiseFilter;
            Assert.That(ff.Intensity, Is.EqualTo(50));
            Assert.That(ff.Strength, Is.EqualTo(60));
            Assert.That(ff.Seed, Is.EqualTo(70));
            Assert.That(ff.Type, Is.EqualTo(AddNoiseFilter.NoiseType.Monochrome));
        }

        [Test]
        public void TestAddNoiseParCnt() => Assert.That(Common.ParamCount(typeof(AddNoiseFilter)), Is.EqualTo(4));


        [Test]
        public void TestMedian()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Median");
            Assert.That(f, Is.InstanceOf<MedianFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Strength", "50");
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "3");
            MedianFilter ff = f as MedianFilter;
            Assert.That(ff.Strength, Is.EqualTo(50));
            Assert.That(ff.Radius, Is.EqualTo(3));
        }

        [Test]
        public void TestMedianParCnt() => Assert.That(Common.ParamCount(typeof(MedianFilter)), Is.EqualTo(2));
    }
}
