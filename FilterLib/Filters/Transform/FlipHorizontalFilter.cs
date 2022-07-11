using FilterLib.Reporting;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Flip the image horizontally (along a vertical axis).
    /// </summary>
    [Filter]
    public sealed class FlipHorizontalFilter : FilterInPlaceBase
    {
        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            int width_3 = image.Width * 3;
            int width_div2_3 = image.Width / 2 * 3;
            fixed (byte* start = image)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    byte* row = start + y * width_3;
                    for (int x = 0; x < width_div2_3; x += 3)
                    {
                        for (int c = 0; c < 3; ++c)
                        {
                            (row[width_3 - x - 3 + c], row[x + c]) = (row[x + c], row[width_3 - x - 3 + c]);
                        }
                    }
                    reporter?.Report(y, 0, image.Height - 1);
                }
            }
            reporter?.Done();
        }
    }
}
