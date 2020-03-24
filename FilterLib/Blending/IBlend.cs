using Bitmap = System.Drawing.Bitmap;

namespace FilterLib.Blending
{
    /// <summary>
    /// Blend mode interface.
    /// </summary>
    public interface IBlend
    {
        /// <summary>
        /// Blend two images together creating a new image.
        /// </summary>
        /// <param name="bottom">Bottom image</param>
        /// <param name="top">Top image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>Blended image</returns>
        Bitmap Apply(Bitmap bottom, Bitmap top, Reporting.IReporter reporter = null);
    }
}