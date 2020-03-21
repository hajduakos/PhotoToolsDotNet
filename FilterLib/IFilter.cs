using Bitmap = System.Drawing.Bitmap;

namespace FilterLib
{
    public interface IFilter
    {
        Bitmap Apply(Bitmap image, Reporting.IReporter reporter = null);
    }
}
