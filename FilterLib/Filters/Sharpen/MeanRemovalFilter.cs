using FilterLib.Reporting;
using FilterLib.Util;
using System.Drawing;

namespace FilterLib.Filters.Sharpen
{
    /// <summary>
    /// Mean removal filter.
    /// </summary>
    [Filter]
    public sealed class MeanRemovalFilter : FilterInPlaceBase
    {
        private readonly Other.ConvolutionFilter conv =
               new(new Conv3x3(-1, -1, -1, -1, 9, -1, -1, -1, -1, 1, 0));

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
