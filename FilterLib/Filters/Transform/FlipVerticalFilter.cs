using FilterLib.Reporting;
using Parallel = System.Threading.Tasks.Parallel;

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
            object reporterLock = new();
            int progress = 0;
            int width_3 = image.Width * 3;
            int height_div2 = image.Height / 2;
            fixed (byte* start = image)
            {
                byte* start0 = start;
                Parallel.For(0, height_div2, y =>
                {
                    byte* row1 = start0 + y * width_3;
                    byte* row2 = start0 + (image.Height - y - 1) * width_3;
                    for (int x = 0; x < width_3; ++x)
                    {
                        (row2[x], row1[x]) = (row1[x], row2[x]);
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, height_div2);
                });
            }
            reporter?.Done();
        }
    }
}
