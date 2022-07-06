namespace FilterLib.Blending
{
    /// <summary>
    /// Blend mode interface that can be applied in place.
    /// </summary>
    public interface IBlendInPlace : IBlend
    {
        /// <summary>
        /// Blend two images together, the result is in the bottom image.
        /// </summary>
        /// <param name="bottom">Bottom image (will be changed)</param>
        /// <param name="top">Top image</param>
        /// <param name="reporter">Reporter (optional)</param>
        void ApplyInPlace(Image bottom, Image top, Reporting.IReporter reporter = null);
    }
}