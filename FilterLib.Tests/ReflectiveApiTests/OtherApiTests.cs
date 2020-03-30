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
            Assert.IsInstanceOf<ConvertToPolarFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Phase", "123.45");
            ConvertToPolarFilter ff = f as ConvertToPolarFilter;
            Assert.AreEqual(123.45f, ff.Phase);
        }

        [Test]
        public void TestConvolution()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Convolution");
            Assert.IsInstanceOf<ConvolutionFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Matrix", "[1 2 3 ; 4 5 6 ; 7 8 9] / 10 + 11");
            ConvolutionFilter ff = f as ConvolutionFilter;
            Assert.AreEqual(1, ff.Matrix[0, 0]);
            Assert.AreEqual(2, ff.Matrix[1, 0]);
            Assert.AreEqual(3, ff.Matrix[2, 0]);
            Assert.AreEqual(4, ff.Matrix[0, 1]);
            Assert.AreEqual(5, ff.Matrix[1, 1]);
            Assert.AreEqual(6, ff.Matrix[2, 1]);
            Assert.AreEqual(7, ff.Matrix[0, 2]);
            Assert.AreEqual(8, ff.Matrix[1, 2]);
            Assert.AreEqual(9, ff.Matrix[2, 2]);
            Assert.AreEqual(10, ff.Matrix.Divisor);
            Assert.AreEqual(11, ff.Matrix.Bias);
        }

        [Test]
        public void TestEquirectangularToStereographic()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("EquirectangularToStereographic");
            Assert.IsInstanceOf<EquirectangularToStereographicFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "AOV", "123.45");
            ReflectiveApi.SetFilterPropertyByName(f, "Spin", "67.89");
            EquirectangularToStereographicFilter ff = f as EquirectangularToStereographicFilter;
            Assert.AreEqual(123.45f, ff.AOV);
            Assert.AreEqual(67.89f, ff.Spin);
        }
    }
}
