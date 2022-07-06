using Bitmap = System.Drawing.Bitmap;

namespace FilterLib.Filters
{
    /// <summary>
    /// Base class for image filters that can be applied on the original image.
    /// </summary>
    public abstract class FilterInPlaceBase : FilterBase, IFilterInPlace
    {
        /// <inheritdoc/>
        public override Image Apply(Image image, Reporting.IReporter reporter = null)
        {
            Image result = (Image)image.Clone();
            ApplyInPlace(result, reporter);
            return result;
        }

        /// <inheritdoc/>
        public abstract void ApplyInPlace(Image image, Reporting.IReporter reporter = null);
    }
}
