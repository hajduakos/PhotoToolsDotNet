﻿using FilterLib.Reporting;
using FilterLib.Util;
using System.Drawing;

namespace FilterLib.Filters.Sharpen
{
    /// <summary>
    /// Sharpen filter.
    /// </summary>
    [Filter]
    public sealed class SharpenFilter : FilterInPlaceBase
    {
        private readonly Other.ConvolutionFilter conv =
            new Other.ConvolutionFilter(new Conv3x3(0, -2, 0, -2, 11, -2, 0, -2, 0, 3, 0));

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}