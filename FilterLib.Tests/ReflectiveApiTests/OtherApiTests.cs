using FilterLib.Filters;
using FilterLib.Filters.Other;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class OtherApiTests
    {
        [Test]
        public void TestConvertToPolar()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("ConvertToPolar");
            Assert.That(f, Is.InstanceOf<ConvertToPolarFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Phase", "123.45");
            ConvertToPolarFilter ff = f as ConvertToPolarFilter;
            Assert.That(ff.Phase, Is.EqualTo(123.45f));
        }

        [Test]
        public void TestConvertToPolarParCnt() => Assert.That(Common.ParamCount(typeof(ConvertToPolarFilter)), Is.EqualTo(1));

        [Test]
        public void TestConvolution()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Convolution");
            Assert.That(f, Is.InstanceOf<ConvolutionFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Matrix", "[[1, 2, 3], [4, 5, 6], [7, 8, 9]] / 10 + 11");
            ConvolutionFilter ff = f as ConvolutionFilter;
            Assert.That(ff.Matrix[0, 0], Is.EqualTo(1));
            Assert.That(ff.Matrix[0, 1], Is.EqualTo(2));
            Assert.That(ff.Matrix[0, 2], Is.EqualTo(3));
            Assert.That(ff.Matrix[1, 0], Is.EqualTo(4));
            Assert.That(ff.Matrix[1, 1], Is.EqualTo(5));
            Assert.That(ff.Matrix[1, 2], Is.EqualTo(6));
            Assert.That(ff.Matrix[2, 0], Is.EqualTo(7));
            Assert.That(ff.Matrix[2, 1], Is.EqualTo(8));
            Assert.That(ff.Matrix[2, 2], Is.EqualTo(9));
            Assert.That(ff.Matrix.Divisor, Is.EqualTo(10));
            Assert.That(ff.Matrix.Bias, Is.EqualTo(11));
        }

        [Test]
        public void TestConvolutionParCnt() => Assert.That(Common.ParamCount(typeof(ConvolutionFilter)), Is.EqualTo(1));

        [Test]
        public void TestEquirectangularToStereographic()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("EquirectangularToStereographic");
            Assert.That(f, Is.InstanceOf<EquirectangularToStereographicFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "AOV", "123.45");
            ReflectiveApi.SetFilterPropertyByName(f, "Spin", "67.89");
            ReflectiveApi.SetFilterPropertyByName(f, "Interpolation", "Bilinear");
            EquirectangularToStereographicFilter ff = f as EquirectangularToStereographicFilter;
            Assert.That(ff.AOV, Is.EqualTo(123.45f));
            Assert.That(ff.Spin, Is.EqualTo(67.89f));
            Assert.That(ff.Interpolation, Is.EqualTo(Util.InterpolationMode.Bilinear));
        }

        [Test]
        public void TestEquirectangularToStereographicParCnt() =>
            Assert.That(Common.ParamCount(typeof(EquirectangularToStereographicFilter)), Is.EqualTo(3));

        [Test]
        public void TestWaves()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Waves");
            Assert.That(f, Is.InstanceOf<WavesFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Wavelength", "50%");
            ReflectiveApi.SetFilterPropertyByName(f, "Amplitude", "40px");
            ReflectiveApi.SetFilterPropertyByName(f, "Direction", "Vertical");
            WavesFilter ff = f as WavesFilter;
            Assert.That(ff.Wavelength.ToAbsolute(400), Is.EqualTo(200));
            Assert.That(ff.Amplitude.ToAbsolute(0), Is.EqualTo(40));
            Assert.That(ff.Direction, Is.EqualTo(WavesFilter.WaveDirection.Vertical));
        }

        [Test]
        public void TestWavesParCnt() => Assert.That(Common.ParamCount(typeof(WavesFilter)), Is.EqualTo(3));
    }
}
