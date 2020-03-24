using FilterLib.Blending;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class BlendApiTests
    {
        [Test]
        public void ColorDodgeBlend() =>
            Assert.IsInstanceOf<ColorDodgeBlend>(ReflectiveApi.ConstructBlendByName("ColorDodge"));

        [Test]
        public void DarkenBlend() =>
            Assert.IsInstanceOf<DarkenBlend>(ReflectiveApi.ConstructBlendByName("Darken"));

        [Test]
        public void LightenBlend() =>
            Assert.IsInstanceOf<LightenBlend>(ReflectiveApi.ConstructBlendByName("Lighten"));

        [Test]
        public void MultiplyBlend() =>
            Assert.IsInstanceOf<MultiplyBlend>(ReflectiveApi.ConstructBlendByName("Multiply"));

        [Test]
        public void NormalBlend() =>
            Assert.IsInstanceOf<NormalBlend>(ReflectiveApi.ConstructBlendByName("Normal"));

        [Test]
        public void ScreenBlend() =>
            Assert.IsInstanceOf<ScreenBlend>(ReflectiveApi.ConstructBlendByName("Screen"));
    }
}
