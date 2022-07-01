using Bitmap = System.Drawing.Bitmap;

namespace FilterLib.Filters
{
    /// <summary>
    /// Base class for image filters that can be applied on the original image.
    /// </summary>
    public abstract class FilterInPlaceBase : FilterBase, IFilterInPlace
    {
        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        public override Bitmap Apply(Bitmap image, Reporting.IReporter reporter = null)
        {
            Bitmap result = (Bitmap)image.Clone();
            ApplyInPlace(result, reporter);
            return result;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public abstract void ApplyInPlace(Bitmap image, Reporting.IReporter reporter = null);
    }
}
