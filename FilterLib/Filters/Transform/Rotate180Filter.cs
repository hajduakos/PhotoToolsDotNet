using FilterLib.Reporting;
using Parallel = System.Threading.Tasks.Parallel;

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
            object reporterLock = new();
            int progress = 0;

            int width_3 = image.Width * 3;
            int hDiv2 = image.Height / 2;
            fixed (byte* start = image)
            {
                byte* start0 = start;
                Parallel.For(0, hDiv2, y =>
                {
                    byte* row1 = start0 + (y * width_3);
                    byte* row2 = start0 + ((image.Height - y - 1) * width_3);
                    for (int x = 0; x < width_3; x += 3)
                    {
                        for (int c = 0; c < 3; ++c)
                        {
                            (row2[width_3 - x - 3 + c], row1[x + c]) = (row1[x + c], row2[width_3 - x - 3 + c]);
                        }
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, hDiv2);
                });
            }
            reporter?.Done();
        }
    }
}
