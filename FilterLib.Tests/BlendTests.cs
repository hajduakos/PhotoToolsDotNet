using FilterLib.Blending;
using NUnit.Framework;
using System.Collections.Generic;

namespace FilterLib.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class BlendTests
    {
        internal static IEnumerable<TestCaseData> Data()
        {
            yield return new TestCaseData("_input.bmp", new ColorBlend(0), 2);
            yield return new TestCaseData("ColorBlend_80.bmp", new ColorBlend(80), 2);
            yield return new TestCaseData("ColorBlend_100.bmp", new ColorBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new ColorBurnBlend(0), 2);
            yield return new TestCaseData("ColorBurnBlend_80.bmp", new ColorBurnBlend(80), 2);
            yield return new TestCaseData("ColorBurnBlend_100.bmp", new ColorBurnBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new ColorDodgeBlend(0), 2);
            yield return new TestCaseData("ColorDodgeBlend_80.bmp", new ColorDodgeBlend(80), 2);
            yield return new TestCaseData("ColorDodgeBlend_100.bmp", new ColorDodgeBlend(100), 2);
        
            yield return new TestCaseData("_input.bmp", new DarkenBlend(0), 2);
            yield return new TestCaseData("DarkenBlend_80.bmp", new DarkenBlend(80), 2);
            yield return new TestCaseData("DarkenBlend_100.bmp", new DarkenBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new DarkerColorBlend(0), 2);
            yield return new TestCaseData("DarkerColorBlend_80.bmp", new DarkerColorBlend(80), 2);
            yield return new TestCaseData("DarkerColorBlend_100.bmp", new DarkerColorBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new DifferenceBlend(0), 2);
            yield return new TestCaseData("DifferenceBlend_80.bmp", new DifferenceBlend(80), 2);
            yield return new TestCaseData("DifferenceBlend_100.bmp", new DifferenceBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new HardLightBlend(0), 2);
            yield return new TestCaseData("HardLightBlend_80.bmp", new HardLightBlend(80), 2);
            yield return new TestCaseData("HardLightBlend_100.bmp", new HardLightBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new HueBlend(0), 2);
            yield return new TestCaseData("HueBlend_80.bmp", new HueBlend(80), 2);
            yield return new TestCaseData("HueBlend_100.bmp", new HueBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new LightenBlend(0), 2);
            yield return new TestCaseData("LightenBlend_80.bmp", new LightenBlend(80), 2);
            yield return new TestCaseData("LightenBlend_100.bmp", new LightenBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new LighterColorBlend(0), 2);
            yield return new TestCaseData("LighterColorBlend_80.bmp", new LighterColorBlend(80), 2);
            yield return new TestCaseData("LighterColorBlend_100.bmp", new LighterColorBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new LighterColorBlend(0), 2);
            yield return new TestCaseData("LightnessBlend_80.bmp", new LightnessBlend(80), 2);
            yield return new TestCaseData("LightnessBlend_100.bmp", new LightnessBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new LinearBurnBlend(0), 2);
            yield return new TestCaseData("LinearBurnBlend_80.bmp", new LinearBurnBlend(80), 2);
            yield return new TestCaseData("LinearBurnBlend_100.bmp", new LinearBurnBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new LinearDodgeBlend(0), 2);
            yield return new TestCaseData("LinearDodgeBlend_80.bmp", new LinearDodgeBlend(80), 2);
            yield return new TestCaseData("LinearDodgeBlend_100.bmp", new LinearDodgeBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new MultiplyBlend(0), 2);
            yield return new TestCaseData("MultiplyBlend_80.bmp", new MultiplyBlend(80), 2);
            yield return new TestCaseData("MultiplyBlend_100.bmp", new MultiplyBlend(100), 2);
        
            yield return new TestCaseData("_input.bmp", new NormalBlend(0), 1);
            yield return new TestCaseData("NormalBlend_25.bmp", new NormalBlend(25), 1);
            yield return new TestCaseData("NormalBlend_50.bmp", new NormalBlend(50), 1);
            yield return new TestCaseData("NormalBlend_75.bmp", new NormalBlend(75), 1);
            yield return new TestCaseData("_input2.bmp", new NormalBlend(100), 1);

            yield return new TestCaseData("_input.bmp", new OverlayBlend(0), 2);
            yield return new TestCaseData("OverlayBlend_80.bmp", new OverlayBlend(80), 2);
            yield return new TestCaseData("OverlayBlend_100.bmp", new OverlayBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new ScreenBlend(0), 2);
            yield return new TestCaseData("ScreenBlend_80.bmp", new ScreenBlend(80), 2);
            yield return new TestCaseData("ScreenBlend_100.bmp", new ScreenBlend(100), 2);

            yield return new TestCaseData("_input.bmp", new SaturationBlend(0), 2);
            yield return new TestCaseData("SaturationBlend_80.bmp", new SaturationBlend(80), 2);
            yield return new TestCaseData("SaturationBlend_100.bmp", new SaturationBlend(100), 2);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IBlend blend, int tolerance) =>
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", expected, blend, tolerance));

        [Test]
        public void TestBlendWithSelf()
        {
            Image img = new(1, 1);
            (img[0, 0, 0], img[0, 0, 1], img[0, 0, 2]) = (1, 2, 3);
            Image result = new NormalBlend(100).Apply(img, img);
            Assert.AreEqual(1, result[0, 0, 0]);
            Assert.AreEqual(2, result[0, 0, 1]);
            Assert.AreEqual(3, result[0, 0, 2]);
        }
    }
}
