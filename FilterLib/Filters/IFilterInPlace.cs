﻿using Bitmap = System.Drawing.Bitmap;

namespace FilterLib.Filters
{
    /// <summary>
    /// Interface for image filters that can be applied on the original image.
    /// </summary>
    public interface IFilterInPlace : IFilter
    {
        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        void ApplyInPlace(Bitmap image, Reporting.IReporter reporter = null);
    }
}
