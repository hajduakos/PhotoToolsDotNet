using Bitmap = System.Drawing.Bitmap;

namespace FilterLib.Filters
{
    /// <summary>
    /// The basic interface for image filters.
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
