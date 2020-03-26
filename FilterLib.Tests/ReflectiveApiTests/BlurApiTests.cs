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
    }
}
