using FilterLib.Reporting;
using Parallel = System.Threading.Tasks.Parallel;

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
            object reporterLock = new();
            int progress = 0;
            int width_3 = image.Width * 3;
            int width_div2_3 = image.Width / 2 * 3;
            fixed (byte* start = image)
            {
                byte* start0 = start;
                Parallel.For(0, image.Height, y =>
                {
                    byte* row = start0 + y * width_3;
                    for (int x = 0; x < width_div2_3; x += 3)
                    {
                        for (int c = 0; c < 3; ++c)
                        {
                            (row[width_3 - x - 3 + c], row[x + c]) = (row[x + c], row[width_3 - x - 3 + c]);
                        }
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }
            reporter?.Done();
        }
    }
}
