using FilterLib.Filters;
using FilterLib.Filters.Blur;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class BlurApiTests
    {

        [Test]
        public void TestBlur() =>
            Assert.IsInstanceOf<BlurFilter>(ReflectiveApi.ConstructFilterByName("Blur"));

        [Test]
        public void TestBlurParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(BlurFilter)));

        [Test]
        public void TestBoxBlur()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("BoxBlur");
            Assert.IsInstanceOf<BoxBlurFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "RadiusX", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "RadiusY", "20");
            BoxBlurFilter ff = f as BoxBlurFilter;
            Assert.AreEqual(10, ff.RadiusX);
            Assert.AreEqual(20, ff.RadiusY);
        }

        [Test]
        public void TestBoxBlurParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(BoxBlurFilter)));

        [Test]
        public void TestGaussianBlur()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("GaussianBlur");
            Assert.IsInstanceOf<GaussianBlurFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "10");
            GaussianBlurFilter ff = f as GaussianBlurFilter;
            Assert.AreEqual(10, ff.Radius);
        }

        [Test]
        public void TestGaussianBlurParCnt() => Assert.AreEqual(1, Common.ParamCount(typeof(GaussianBlurFilter)));

        [Test]
        public void TestMotionBlur()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("MotionBlur");
            Assert.IsInstanceOf<MotionBlurFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Length", "10");
            ReflectiveApi.SetFilterPropertyByName(f, "Angle", "12.34");
            MotionBlurFilter ff = f as MotionBlurFilter;
            Assert.AreEqual(10, ff.Length);
            Assert.AreEqual(12.34f, ff.Angle, 0.01f);
        }

        [Test]
        public void TestMotionBlurParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(MotionBlurFilter)));

        [Test]
        public void TestSpinBlur()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("SpinBlur");
            Assert.IsInstanceOf<SpinBlurFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "CenterX", "10px");
            ReflectiveApi.SetFilterPropertyByName(f, "CenterY", "20px");
            ReflectiveApi.SetFilterPropertyByName(f, "Angle", "30");
            ReflectiveApi.SetFilterPropertyByName(f, "Samples", "40");
            SpinBlurFilter ff = f as SpinBlurFilter;
            Assert.AreEqual(10, ff.CenterX.ToAbsolute(100));
            Assert.AreEqual(20, ff.CenterY.ToAbsolute(100));
            Assert.AreEqual(30, ff.Angle);
            Assert.AreEqual(40, ff.Samples);
        }

        [Test]
        public void TestSpinBlurParCnt() => Assert.AreEqual(4, Common.ParamCount(typeof(SpinBlurFilter)));

        [Test]
        public void TestZoomBlur()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("ZoomBlur");
            Assert.IsInstanceOf<ZoomBlurFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "CenterX", "10px");
            ReflectiveApi.SetFilterPropertyByName(f, "CenterY", "20px");
            ReflectiveApi.SetFilterPropertyByName(f, "Amount", "30");
            ZoomBlurFilter ff = f as ZoomBlurFilter;
            Assert.AreEqual(10, ff.CenterX.ToAbsolute(100));
            Assert.AreEqual(20, ff.CenterY.ToAbsolute(100));
            Assert.AreEqual(30, ff.Amount);
        }

        [Test]
        public void TestZoomBlurParCnt() => Assert.AreEqual(3, Common.ParamCount(typeof(ZoomBlurFilter)));
    }
}
