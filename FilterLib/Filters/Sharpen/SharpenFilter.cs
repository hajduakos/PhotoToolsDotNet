using FilterLib.Reporting;
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
            new(new Conv3x3(0, -2, 0, -2, 11, -2, 0, -2, 0, 3, 0));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
