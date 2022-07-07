namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Apply a small amount of blur with a 3x3 convolution.
    /// </summary>
    [Filter]
    public class BlurFilter : FilterInPlaceBase
    {
        private readonly Other.ConvolutionFilter conv = new(new Util.Conv3x3(1, 1, 1, 1, 8, 1, 1, 1, 1, 16, 0));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, Reporting.IReporter reporter = null) => conv.ApplyInPlace(image, reporter);
    }
}
