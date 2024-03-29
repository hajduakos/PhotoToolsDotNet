﻿namespace FilterLib.Blending
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
        Image Apply(Image bottom, Image top, Reporting.IReporter reporter = null);

        /// <summary>
        /// Opacity of the top image [0:100]
        /// </summary>
        public int Opacity { get; set; }
    }
}