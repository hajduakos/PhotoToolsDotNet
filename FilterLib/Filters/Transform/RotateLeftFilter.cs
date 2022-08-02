using FilterLib.Reporting;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate the image left (counter clockwise) in a lossless way.
    /// </summary>
    [Filter]
    public sealed class RotateLeftFilter : FilterBase
    {
        /// <inheritdoc/>
        public override unsafe Image Apply(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            Image rotated = new(image.Height, image.Width);
            int oldWidth_3 = image.Width * 3;
            int newWidth_3 = rotated.Width * 3;
            fixed (byte* oldStart = image, newStart = rotated)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                Parallel.For(0, image.Height, y =>
                {
                    byte* oldRow = oldStart0 + y * oldWidth_3;
                    for (int x = 0; x < oldWidth_3; x += 3)
                    {
                        int idx = (rotated.Height - x / 3 - 1) * newWidth_3 + y * 3;
                        newStart0[idx] = oldRow[x];
                        newStart0[idx + 1] = oldRow[x + 1];
                        newStart0[idx + 2] = oldRow[x + 2];
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }
            reporter?.Done();
            return rotated;
        }
    }
}
