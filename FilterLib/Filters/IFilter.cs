using Bitmap = System.Drawing.Bitmap;

namespace FilterLib.Filters
{
    /// <summary>
    /// The basic interface for image filters. If a filter can be applied on
    /// the same image (in place) consider using the derived
    /// <see cref="IFilterInPlace"/> interface.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        Bitmap Apply(Bitmap image, Reporting.IReporter reporter = null);
    }
}
