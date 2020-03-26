using FilterLib.Filters.Generate;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class GenerateTests
    {
        [Test]
        public void TestMarble()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Marble_2_4_5_6_0.bmp", new MarbleFilter(2, 4, 5, 6, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Marble_3_7_2_5_0.bmp", new MarbleFilter(3, 7, 2, 5, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Marble_5_2_10_8_0.bmp", new MarbleFilter(5, 2, 10, 8, 0), 1));
        }

        [Test]
        public void TestTurbulence()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Turbulence_1_0.bmp", new TurbulenceFilter(1, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Turbulence_5_0.bmp", new TurbulenceFilter(5, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Turbulence_10_0.bmp", new TurbulenceFilter(10, 0), 1));
        }

        [Test]
        public void TestWoodRings()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "WoodRings_2_0.05_8_0.bmp", new WoodRingsFilter(2, 0.05f, 8, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "WoodRings_5_0.1_6_0.bmp", new WoodRingsFilter(5, 0.1f, 6, 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "WoodRings_8_0.3_7_0.bmp", new WoodRingsFilter(8, 0.3f, 7, 0), 1));
        }
    }
}
