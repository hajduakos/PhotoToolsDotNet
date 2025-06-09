using FilterLib.Filters;
using FilterLib.Filters.Transform;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class TransformApiTests
    {

        [Test]
        public void TestBoxDownscale()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("BoxDownscale");
            Assert.That(f, Is.InstanceOf<BoxDownscaleFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "100px");
            ReflectiveApi.SetFilterPropertyByName(f, "Height", "50%");
            BoxDownscaleFilter ff = f as BoxDownscaleFilter;
            Assert.That(ff.Width.ToAbsolute(500), Is.EqualTo(100));
            Assert.That(ff.Height.ToAbsolute(500), Is.EqualTo(250));
        }

        [Test]
        public void TestBoxDownscaleParCnt() => Assert.That(Common.ParamCount(typeof(BoxDownscaleFilter)), Is.EqualTo(2));

        [Test]
        public void TestCrop()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Crop");
            Assert.That(f, Is.InstanceOf<CropFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "X", "10px");
            ReflectiveApi.SetFilterPropertyByName(f, "Y", "25%");
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "100px");
            ReflectiveApi.SetFilterPropertyByName(f, "Height", "50%");
            CropFilter ff = f as CropFilter;
            Assert.That(ff.X.ToAbsolute(500), Is.EqualTo(10));
            Assert.That(ff.Y.ToAbsolute(500), Is.EqualTo(125));
            Assert.That(ff.Width.ToAbsolute(500), Is.EqualTo(100));
            Assert.That(ff.Height.ToAbsolute(500), Is.EqualTo(250));
        }

        [Test]
        public void TestCropParCnt() => Assert.That(Common.ParamCount(typeof(CropFilter)), Is.EqualTo(4));


        [Test]
        public void TestFlipHorizontal() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("FlipHorizontal"), Is.InstanceOf<FlipHorizontalFilter>());

        [Test]
        public void TestFlipHorizontalParCnt() => Assert.That(Common.ParamCount(typeof(FlipHorizontalFilter)), Is.EqualTo(0));

        [Test]
        public void TestFlipVertical() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("FlipVertical"), Is.InstanceOf<FlipVerticalFilter>());

        [Test]
        public void TestFlipVerticalParCnt() => Assert.That(Common.ParamCount(typeof(FlipVerticalFilter)), Is.EqualTo(0));

        [Test]
        public void TestPerspective()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Perspective");
            Assert.That(f, Is.InstanceOf<PerspectiveFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Scale", "12.34");
            ReflectiveApi.SetFilterPropertyByName(f, "Direction", "Vertical");
            ReflectiveApi.SetFilterPropertyByName(f, "Interpolation", "Bilinear");
            PerspectiveFilter ff = f as PerspectiveFilter;
            Assert.That(ff.Scale, Is.EqualTo(12.34f));
            Assert.That(ff.Direction, Is.EqualTo(PerspectiveFilter.PerspectiveDirection.Vertical));
            Assert.That(ff.Interpolation, Is.EqualTo(Util.InterpolationMode.Bilinear));
        }

        [Test]
        public void TestPerspectiveParCnt() => Assert.That(Common.ParamCount(typeof(PerspectiveFilter)), Is.EqualTo(3));


        [Test]
        public void TestResize()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Resize");
            Assert.That(f, Is.InstanceOf<ResizeFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Width", "100px");
            ReflectiveApi.SetFilterPropertyByName(f, "Height", "50%");
            ReflectiveApi.SetFilterPropertyByName(f, "Interpolation", "NearestNeighbor");
            ResizeFilter ff = f as ResizeFilter;
            Assert.That(ff.Width.ToAbsolute(500), Is.EqualTo(100));
            Assert.That(ff.Height.ToAbsolute(500), Is.EqualTo(250));
            Assert.That(ff.Interpolation, Is.EqualTo(Util.InterpolationMode.NearestNeighbor));
        }

        [Test]
        public void TestResizeParCnt() => Assert.That(Common.ParamCount(typeof(ResizeFilter)), Is.EqualTo(3));

        [Test]
        public void TestRotate()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Rotate");
            Assert.That(f, Is.InstanceOf<RotateFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Angle", "123.45");
            ReflectiveApi.SetFilterPropertyByName(f, "Crop", "Fill");
            ReflectiveApi.SetFilterPropertyByName(f, "Interpolation", "NearestNeighbor");
            RotateFilter ff = f as RotateFilter;
            Assert.That(ff.Angle, Is.EqualTo(123.45f));
            Assert.That(ff.Crop, Is.EqualTo(RotateFilter.CropMode.Fill));
            Assert.That(ff.Interpolation, Is.EqualTo(Util.InterpolationMode.NearestNeighbor));
        }

        [Test]
        public void TestRotateParCnt() => Assert.That(Common.ParamCount(typeof(RotateFilter)), Is.EqualTo(3));

        [Test]
        public void TestRotate180() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("Rotate180"), Is.InstanceOf<Rotate180Filter>());

        [Test]
        public void TestRotate180ParCnt() => Assert.That(Common.ParamCount(typeof(Rotate180Filter)), Is.EqualTo(0));

        [Test]
        public void TestRotateLeft() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("RotateLeft"), Is.InstanceOf<RotateLeftFilter>());

        [Test]
        public void TestRotateLeftParCnt() => Assert.That(Common.ParamCount(typeof(RotateLeftFilter)), Is.EqualTo(0));

        [Test]
        public void TestRotateRight() =>
            Assert.That(ReflectiveApi.ConstructFilterByName("RotateRight"), Is.InstanceOf<RotateRightFilter>());

        [Test]
        public void TestRotateRightParCnt() => Assert.That(Common.ParamCount(typeof(RotateRightFilter)), Is.EqualTo(0));

        [Test]
        public void TestSkew()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Skew");
            Assert.That(f, Is.InstanceOf<SkewFilter>());
            ReflectiveApi.SetFilterPropertyByName(f, "Angle", "12.34");
            ReflectiveApi.SetFilterPropertyByName(f, "Direction", "Vertical");
            SkewFilter ff = f as SkewFilter;
            Assert.That(ff.Angle, Is.EqualTo(12.34f));
            Assert.That(ff.Direction, Is.EqualTo(SkewFilter.SkewDirection.Vertical));
        }

        [Test]
        public void TestSkewParCnt() => Assert.That(Common.ParamCount(typeof(SkewFilter)), Is.EqualTo(2));
    }
}
