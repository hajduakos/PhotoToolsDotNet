using FilterLib.Blending.Cancelation;
using FilterLib.Blending.Component;
using FilterLib.Blending.Contrast;
using FilterLib.Blending.Darken;
using FilterLib.Blending.Inversion;
using FilterLib.Blending.Lighten;
using FilterLib.Blending.Normal;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class BlendApiTests
    {
        [Test]
        public void TestColorBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Color"), Is.InstanceOf<ColorBlend>());

        [Test]
        public void TestColorBurnBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("ColorBurn"), Is.InstanceOf<ColorBurnBlend>());

        [Test]
        public void TestColorDodgeBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("ColorDodge"), Is.InstanceOf<ColorDodgeBlend>());

        [Test]
        public void TestDarkenBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Darken"), Is.InstanceOf<DarkenBlend>());

        [Test]
        public void TestDarkerColorBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("DarkerColor"), Is.InstanceOf<DarkerColorBlend>());

        [Test]
        public void TestDifferenceBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Difference"), Is.InstanceOf<DifferenceBlend>());

        [Test]
        public void TestDivideBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Divide"), Is.InstanceOf<DivideBlend>());

        [Test]
        public void TestExcludeBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Exclude"), Is.InstanceOf<ExcludeBlend>());

        [Test]
        public void TestHardLightBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("HardLight"), Is.InstanceOf<HardLightBlend>());

        [Test]
        public void TestHardMixBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("HardMix"), Is.InstanceOf<HardMixBlend>());

        [Test]
        public void TestHueBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Hue"), Is.InstanceOf<HueBlend>());

        [Test]
        public void TestLightenBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Lighten"), Is.InstanceOf<LightenBlend>());

        [Test]
        public void TestLighterColorBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("LighterColor"), Is.InstanceOf<LighterColorBlend>());

        [Test]
        public void TestLightnessBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Lightness"), Is.InstanceOf<LightnessBlend>());

        [Test]
        public void TestLinearBurnBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("LinearBurn"), Is.InstanceOf<LinearBurnBlend>());

        [Test]
        public void TestLinearDodgeBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("LinearDodge"), Is.InstanceOf<LinearDodgeBlend>());

        [Test]
        public void TestLinearLightBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("LinearLight"), Is.InstanceOf<LinearLightBlend>());

        [Test]
        public void TestMultiplyBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Multiply"), Is.InstanceOf<MultiplyBlend>());

        [Test]
        public void TestNormalBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Normal"), Is.InstanceOf<NormalBlend>());

        [Test]
        public void TestOverlayBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Overlay"), Is.InstanceOf<OverlayBlend>());

        [Test]
        public void TestPinLightBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("PinLight"), Is.InstanceOf<PinLightBlend>());

        [Test]
        public void TestSaturationBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Saturation"), Is.InstanceOf<SaturationBlend>());

        [Test]
        public void TestScreenBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Screen"), Is.InstanceOf<ScreenBlend>());

        [Test]
        public void TestSoftLightBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("SoftLight"), Is.InstanceOf<SoftLightBlend>());

        [Test]
        public void TestSubtractBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("Subtract"), Is.InstanceOf<SubtractBlend>());

        [Test]
        public void TestVividLightBlend() =>
            Assert.That(ReflectiveApi.ConstructBlendByName("VividLight"), Is.InstanceOf<VividLightBlend>());
    }
}
