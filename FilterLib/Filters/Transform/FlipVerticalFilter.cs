using FilterLib.Reporting;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Flip the image vertically (along a horizontal axis).
    /// </summary>
    [Filter]
    public sealed class FlipVerticalFilter : FilterInPlaceBase
    {
        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            int width_3 = image.Width * 3;
            int height_div2 = image.Height / 2;
            fixed (byte* start = image)
            {
                for (int y = 0; y < height_div2; ++y)
                {
                    byte* row1 = start + y * width_3;
                    byte* row2 = start + (image.Height - y - 1) * width_3;
                    for (int x = 0; x < width_3; ++x)
                    {
                        (row2[x], row1[x]) = (row1[x], row2[x]);
                    }
                    reporter?.Report(y, 0, height_div2 - 1);
                }
            }
            reporter?.Done();
        }
    }
}
