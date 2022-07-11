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
            yield return new TestCaseData("_input.bmp", new ColorDodgeBlend(0), 2);
            yield return new TestCaseData("ColorDodgeBlend_80.bmp", new ColorDodgeBlend(80), 2);
            yield return new TestCaseData("ColorDodgeBlend_100.bmp", new ColorDodgeBlend(100), 2);
        
            yield return new TestCaseData("_input.bmp", new DarkenBlend(0), 2);
            yield return new TestCaseData("DarkenBlend_80.bmp", new DarkenBlend(80), 2);
            yield return new TestCaseData("DarkenBlend_100.bmp", new DarkenBlend(100), 2);
        
            yield return new TestCaseData("_input.bmp", new LightenBlend(0), 2);
            yield return new TestCaseData("LightenBlend_80.bmp", new LightenBlend(80), 2);
            yield return new TestCaseData("LightenBlend_100.bmp", new LightenBlend(100), 2);
        
            yield return new TestCaseData("_input.bmp", new MultiplyBlend(0), 2);
            yield return new TestCaseData("MultiplyBlend_80.bmp", new MultiplyBlend(80), 2);
            yield return new TestCaseData("MultiplyBlend_100.bmp", new MultiplyBlend(100), 2);
        
            yield return new TestCaseData("_input.bmp", new NormalBlend(0), 1);
            yield return new TestCaseData("NormalBlend_50.bmp", new NormalBlend(50), 1);
            yield return new TestCaseData("_input2.bmp", new NormalBlend(100), 1);
        
            yield return new TestCaseData("_input.bmp", new ScreenBlend(0), 2);
            yield return new TestCaseData("ScreenBlend_80.bmp", new ScreenBlend(80), 2);
            yield return new TestCaseData("ScreenBlend_100.bmp", new ScreenBlend(100), 2);
        }

        [Test]
        [TestCaseSource("Data")]
        public void Test(string expected, IBlend blend, int tolerance) =>
            Assert.IsTrue(Common.CheckBlend("_input.bmp", "_input2.bmp", expected, blend, tolerance));

        [Test]
        public void TestBlendWithSelf()
        {
            Image img = new Image(1, 1);
            (img[0, 0, 0], img[0, 0, 1], img[0, 0, 2]) = (1, 2, 3);
            Image result = new NormalBlend(100).Apply(img, img);
            Assert.AreEqual(1, result[0, 0, 0]);
            Assert.AreEqual(2, result[0, 0, 1]);
            Assert.AreEqual(3, result[0, 0, 2]);
        }
    }
}
