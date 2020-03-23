using FilterLib.Filters.Noise;
using FilterLib.Scripting;
using NUnit.Framework;

namespace FilterLib.Tests.ParsingTests
{
    public class NoiseParsingTests
    {

        [Test]
        public void TestAddNoiseParse()
        {
            var filters = Parser.Parse(new string[] { "AddNoise" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<AddNoiseFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "AddNoise", "- Intensity: 50", "- Strength: 60", "- Seed: 70", "- Monochrome: true" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<AddNoiseFilter>(filters[0]);
            AddNoiseFilter f = filters[0] as AddNoiseFilter;
            Assert.AreEqual(50, f.Intensity);
            Assert.AreEqual(60, f.Strength);
            Assert.AreEqual(70, f.Seed);
            Assert.IsTrue(f.Monochrome);
        }


        [Test]
        public void TestMedianParse()
        {
            var filters = Parser.Parse(new string[] { "Median" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<MedianFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "Median", "- Strength: 50" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<MedianFilter>(filters[0]);
            MedianFilter f = filters[0] as MedianFilter;
            Assert.AreEqual(50, f.Strength);
        }
    }
}
