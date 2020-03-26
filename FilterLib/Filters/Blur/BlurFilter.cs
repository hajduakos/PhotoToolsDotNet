using FilterLib.Reporting;
using FilterLib.Util;
using System.Drawing;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Blur filter.
    /// </summary>
    [Filter]
    public class BlurFilter : FilterInPlaceBase
    {
        private readonly Other.ConvolutionFilter conv =
            new Other.ConvolutionFilter(new Conv3x3(1, 1, 1, 1, 8, 1, 1, 1, 1, 16, 0));

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
