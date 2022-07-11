using FilterLib.Blending;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class BlendApiTests
    {
        [Test]
        public void TestColorDodgeBlend() =>
            Assert.IsInstanceOf<ColorDodgeBlend>(ReflectiveApi.ConstructBlendByName("ColorDodge"));

        [Test]
        public void TestDarkenBlend() =>
            Assert.IsInstanceOf<DarkenBlend>(ReflectiveApi.ConstructBlendByName("Darken"));

        [Test]
        public void TestDarkerColorBlend() =>
            Assert.IsInstanceOf<DarkerColorBlend>(ReflectiveApi.ConstructBlendByName("DarkerColor"));

        [Test]
        public void TestDifferenceBlend() =>
            Assert.IsInstanceOf<DifferenceBlend>(ReflectiveApi.ConstructBlendByName("Difference"));

        [Test]
        public void TestHueBlend() =>
            Assert.IsInstanceOf<HueBlend>(ReflectiveApi.ConstructBlendByName("Hue"));

        [Test]
        public void TestLightenBlend() =>
            Assert.IsInstanceOf<LightenBlend>(ReflectiveApi.ConstructBlendByName("Lighten"));

        [Test]
        public void TestLighterColorBlend() =>
            Assert.IsInstanceOf<LighterColorBlend>(ReflectiveApi.ConstructBlendByName("LighterColor"));

        [Test]
        public void TestMultiplyBlend() =>
            Assert.IsInstanceOf<MultiplyBlend>(ReflectiveApi.ConstructBlendByName("Multiply"));

        [Test]
        public void TestNormalBlend() =>
            Assert.IsInstanceOf<NormalBlend>(ReflectiveApi.ConstructBlendByName("Normal"));

        [Test]
        public void TestScreenBlend() =>
            Assert.IsInstanceOf<ScreenBlend>(ReflectiveApi.ConstructBlendByName("Screen"));
    }
}
