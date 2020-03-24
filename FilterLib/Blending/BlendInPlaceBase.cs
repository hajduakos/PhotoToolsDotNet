using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;

namespace FilterLib.Blending
{
    /// <summary>
    /// Base class for blend modes applied in place.
    /// </summary>
    public abstract class BlendInPlaceBase : IBlendInPlace
    {
        private int opacity;

        /// <summary>
        /// Opacity [0:100]
        /// </summary>
        public int Opacity
        {
            get { return opacity; }
            set { opacity = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="opacity">Opacity[0;100]</param>
        protected BlendInPlaceBase(int opacity) => this.Opacity = opacity;

        /// <summary>
        /// Blend two images together, the result is in the bottom image.
        /// </summary>
        /// <param name="bottom">Bottom image (will be changed)</param>
        /// <param name="top">Top image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public abstract void ApplyInPlace(Bitmap bottom, Bitmap top, Reporting.IReporter reporter = null);

        /// <summary>
        /// Blend two images together creating a new image.
        /// </summary>
        /// <param name="bottom">Bottom image</param>
        /// <param name="top">Top image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>Blended image</returns>
        public Bitmap Apply(Bitmap bottom, Bitmap top, Reporting.IReporter reporter = null)
        {
            Bitmap result = (Bitmap)bottom.Clone();
            ApplyInPlace(result, top);
            return result;
        }
    }
}