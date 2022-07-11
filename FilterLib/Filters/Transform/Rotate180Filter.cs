using FilterLib.Reporting;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate the image with 180 degrees in a lossless way.
    /// </summary>
    [Filter]
    public sealed class Rotate180Filter : FilterInPlaceBase
    {
        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();

            int width_3 = image.Width * 3;
            int hDiv2 = image.Height / 2;
            fixed (byte* start = image)
            {
                for (int y = 0; y < hDiv2; ++y)
                {
                    byte* row1 = start + (y * width_3);
                    byte* row2 = start + ((image.Height - y - 1) * width_3);
                    for (int x = 0; x < width_3; x += 3)
                    {
                        for (int c = 0; c < 3; ++c)
                        {
                            byte swap = row1[x + c];
                            row1[x + c] = row2[width_3 - x - 3 + c];
                            row2[width_3 - x - 3 + c] = swap;
                        }
                    }
                    reporter?.Report(y, 0, hDiv2 - 1);
                }
            }
            reporter?.Done();
        }
    }
}
