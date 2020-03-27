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
        public void TestGaussianBlur()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("GaussianBlur");
            Assert.IsInstanceOf<GaussianBlurFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Radius", "10");
            GaussianBlurFilter ff = f as GaussianBlurFilter;
            Assert.AreEqual(10, ff.Radius);
        }

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
    }
}
