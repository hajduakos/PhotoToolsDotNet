using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Base class for blend modes applied in place.
    /// </summary>
    public abstract class BlendInPlaceBase : IBlendInPlace
    {
        private int opacity;

        /// <inheritdoc/>
        public int Opacity
        {
            get { return opacity; }
            set { opacity = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="opacity">Opacity[0;100]</param>
        protected BlendInPlaceBase(int opacity) => Opacity = opacity;

        /// <inheritdoc/>
        public abstract void ApplyInPlace(Image bottom, Image top, Reporting.IReporter reporter = null);

        /// <inheritdoc/>
        public Image Apply(Image bottom, Image top, Reporting.IReporter reporter = null)
        {
            Image result = (Image)bottom.Clone();
            ApplyInPlace(result, top, reporter);
            return result;
        }
    }
}