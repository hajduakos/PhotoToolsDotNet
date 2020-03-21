using Bitmap = System.Drawing.Bitmap;

namespace FilterLib
{
    public interface IFilterInPlace
    {
        void ApplyInPlace(Bitmap image, Reporting.IReporter reporter = null);
    }
}
