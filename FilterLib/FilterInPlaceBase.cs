using Bitmap = System.Drawing.Bitmap;

namespace FilterLib
{
    public abstract class FilterInPlaceBase : IFilter, IFilterInPlace
    {
        public Bitmap Apply(Bitmap image, Reporting.IReporter reporter = null)
        {
            Bitmap result = (Bitmap)image.Clone();
            ApplyInPlace(result, reporter);
            return result;
        }

        public abstract void ApplyInPlace(Bitmap image, Reporting.IReporter reporter = null);
    }
}
