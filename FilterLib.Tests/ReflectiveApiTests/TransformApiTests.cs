using FilterLib.Filters;
using FilterLib.Filters.Transform;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class TransformApiTests
    {
        [Test]
        public void TestCrop()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Crop");
            Assert.IsInstanceOf<CropFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "X", "10px");
            ReflectiveApi.SetFilterPropertyByName(f, "Y", "25%");
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "100px");
            ReflectiveApi.SetFilterPropertyByName(f, "Height", "50%");
            CropFilter ff = f as CropFilter;
            Assert.AreEqual(10, ff.X.ToAbsolute(500));
            Assert.AreEqual(125, ff.Y.ToAbsolute(500));
            Assert.AreEqual(100, ff.Width.ToAbsolute(500));
            Assert.AreEqual(250, ff.Height.ToAbsolute(500));
        }

        [Test]
        public void TestCropParCnt() => Assert.AreEqual(4, Common.ParamCount(typeof(CropFilter)));


        [Test]
        public void TestFlipHorizontal() =>
            Assert.IsInstanceOf<FlipHorizontalFilter>(ReflectiveApi.ConstructFilterByName("FlipHorizontal"));

        [Test]
        public void TestFlipHorizontalParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(FlipHorizontalFilter)));

        [Test]
        public void TestFlipVertical() =>
            Assert.IsInstanceOf<FlipVerticalFilter>(ReflectiveApi.ConstructFilterByName("FlipVertical"));

        [Test]
        public void TestFlipVerticalParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(FlipVerticalFilter)));

        [Test]
        public void TestResize()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Resize");
            Assert.IsInstanceOf<ResizeFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "100px");
            ReflectiveApi.SetFilterPropertyByName(f, "Height", "50%");
            ReflectiveApi.SetFilterPropertyByName(f, "Interpolation", "NearestNeighbor");
            ResizeFilter ff = f as ResizeFilter;
            Assert.AreEqual(100, ff.Width.ToAbsolute(500));
            Assert.AreEqual(250, ff.Height.ToAbsolute(500));
            Assert.AreEqual(InterpolationMode.NearestNeighbor, ff.Interpolation);
        }

        [Test]
        public void TestResizeParCnt() => Assert.AreEqual(3, Common.ParamCount(typeof(ResizeFilter)));

        [Test]
        public void TestRotate()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Rotate");
            Assert.IsInstanceOf<RotateFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "Angle", "123.45");
            ReflectiveApi.SetFilterPropertyByName(f, "Crop", "true");
            RotateFilter ff = f as RotateFilter;
            Assert.AreEqual(123.45f, ff.Angle);
            Assert.AreEqual(true, ff.Crop);
        }

        [Test]
        public void TestRotateParCnt() => Assert.AreEqual(2, Common.ParamCount(typeof(RotateFilter)));

        [Test]
        public void TestRotate180() => 
            Assert.IsInstanceOf<Rotate180Filter>(ReflectiveApi.ConstructFilterByName("Rotate180"));

        [Test]
        public void TestRotate180ParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(Rotate180Filter)));

        [Test]
        public void TestRotateLeft() => 
            Assert.IsInstanceOf<RotateLeftFilter>(ReflectiveApi.ConstructFilterByName("RotateLeft"));

        [Test]
        public void TestRotateLeftParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(RotateLeftFilter)));

        [Test]
        public void TestRotateRight() => 
            Assert.IsInstanceOf<RotateRightFilter>(ReflectiveApi.ConstructFilterByName("RotateRight"));

        [Test]
        public void TestRotateRightParCnt() => Assert.AreEqual(0, Common.ParamCount(typeof(RotateRightFilter)));
    }
}
