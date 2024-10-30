using FilterLib.Filters;
using FilterLib.Filters.Blur;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class BlurApiTests
    {

        [Test]
        public void TestBlur() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("Blur"), Is.InstanceOf<BlurFilter>());

        [Test]
        public void TestBlurParCnt() => Assert.That(Common.ParamCount(typeof(BlurFilter)), Is.EqualTo(0));

        [Test]
        public void TestBoxBlur()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("BoxBlur");
            Assert.That(f, Is.InstanceOf<BoxBlurFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "RadiusX", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "RadiusY", "20");
            BoxBlurFilter ff = f as BoxBlurFilter;
            Assert.That(ff.RadiusX, Is.EqualTo(10));
            Assert.That(ff.RadiusY, Is.EqualTo(20));
        }

        [Test]
        public void TestBoxBlurParCnt() => Assert.That(Common.ParamCount(typeof(BoxBlurFilter)), Is.EqualTo(2));

        [Test]
        public void TestGaussianBlur()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("GaussianBlur");
            Assert.That(f, Is.InstanceOf<GaussianBlurFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "10");
            GaussianBlurFilter ff = f as GaussianBlurFilter;
            Assert.That(ff.Radius, Is.EqualTo(10));
        }

        [Test]
        public void TestGaussianBlurParCnt() => Assert.That(Common.ParamCount(typeof(GaussianBlurFilter)), Is.EqualTo(1));

        [Test]
        public void TestMotionBlur()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("MotionBlur");
            Assert.That(f, Is.InstanceOf<MotionBlurFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Length", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Angle", "12.34");
            MotionBlurFilter ff = f as MotionBlurFilter;
            Assert.That(ff.Length, Is.EqualTo(10));
            Assert.That(ff.Angle, Is.EqualTo(12.34f).Within(0.01f));
        }

        [Test]
        public void TestMotionBlurParCnt() => Assert.That(Common.ParamCount(typeof(MotionBlurFilter)), Is.EqualTo(2));

        [Test]
        public void TestSpinBlur()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("SpinBlur");
            Assert.That(f, Is.InstanceOf<SpinBlurFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "CenterX", "10px");
            ReflectiveApi.SetFilterPropertyByName(f, "CenterY", "20px");
            ReflectiveApi.SetFilterPropertyByName(f, "Angle", "30");
            ReflectiveApi.SetFilterPropertyByName(f, "Samples", "40");
            SpinBlurFilter ff = f as SpinBlurFilter;
            Assert.That(ff.CenterX.ToAbsolute(100), Is.EqualTo(10));
            Assert.That(ff.CenterY.ToAbsolute(100), Is.EqualTo(20));
            Assert.That(ff.Angle, Is.EqualTo(30));
            Assert.That(ff.Samples, Is.EqualTo(40));
        }

        [Test]
        public void TestSpinBlurParCnt() => Assert.That(Common.ParamCount(typeof(SpinBlurFilter)), Is.EqualTo(4));

        [Test]
        public void TestZoomBlur()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("ZoomBlur");
            Assert.That(f, Is.InstanceOf<ZoomBlurFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "CenterX", "10px");
            ReflectiveApi.SetFilterPropertyByName(f, "CenterY", "20px");
            ReflectiveApi.SetFilterPropertyByName(f, "Amount", "30");
            ReflectiveApi.SetFilterPropertyByName(f, "MaxSamples", "40");
            ZoomBlurFilter ff = f as ZoomBlurFilter;
            Assert.That(ff.CenterX.ToAbsolute(100), Is.EqualTo(10));
            Assert.That(ff.CenterY.ToAbsolute(100), Is.EqualTo(20));
            Assert.That(ff.Amount, Is.EqualTo(30));
            Assert.That(ff.MaxSamples, Is.EqualTo(40));
        }

        [Test]
        public void TestZoomBlurParCnt() => Assert.That(Common.ParamCount(typeof(ZoomBlurFilter)), Is.EqualTo(4));
    }
}
