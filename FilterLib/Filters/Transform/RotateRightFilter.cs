using FilterLib.Reporting;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate the image right (clockwise) in a lossless way.
    /// </summary>
    [Filter]
    public sealed class RotateRightFilter : FilterBase
    {
        /// <inheritdoc/>
        public override unsafe Image Apply(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            Image rotated = new(image.Height, image.Width);
            int oldWidth_3 = image.Width * 3;
            int newWidth_3 = rotated.Width * 3;
            fixed (byte* oldStart = image, newStart = rotated)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    byte* oldRow = oldStart + (image.Height - 1 - y) * oldWidth_3;
                    for (int x = 0; x < oldWidth_3; x += 3)
                    {
                        int idx = x / 3 * newWidth_3 + y * 3;
                        newStart[idx] = oldRow[x];
                        newStart[idx + 1] = oldRow[x + 1];
                        newStart[idx + 2] = oldRow[x + 2];
                    }
                    reporter?.Report(y, 0, image.Height - 1);
                }
            }
            reporter?.Done();
            return rotated;
        }
    }
}
