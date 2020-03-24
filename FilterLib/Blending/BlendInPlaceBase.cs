using Bitmap = System.Drawing.Bitmap;

namespace FilterLib.Blending
{
    /// <summary>
    /// Base class for blend modes applied in place.
    /// </summary>
    public abstract class BlendInPlaceBase : IBlendInPlace
    {
        public abstract void ApplyInPlace(Bitmap bottom, Bitmap top, Reporting.IReporter reporter = null);

        public Bitmap Apply(Bitmap bottom, Bitmap top, Reporting.IReporter reporter = null)
        {
            Bitmap result = (Bitmap)bottom.Clone();
            ApplyInPlace(result, top);
            return result;
        }
    }
}