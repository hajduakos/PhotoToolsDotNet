using IReporter = FilterLib.Reporting.IReporter;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters
{
    /// <summary>
    /// Base class for filters that process each pixel individually.
    /// </summary>
    public abstract class PerPixelFilterBase : FilterInPlaceBase
    {
        /// <inheritdoc/>
        public override sealed unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            int width_3 = image.Width * 3;
            ApplyStart();
            fixed (byte* start = image)
            {
                byte* start0 = start;
                // Iterate through each pixel and process individually
                Parallel.For(0, image.Height, y =>
                {
                    byte* ptr = start0 + y * width_3;
                    for (int x = 0; x < image.Width; ++x)
                    {
                        ProcessPixel(ptr, ptr + 1, ptr + 2);
                        ptr += 3;
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });

            }
            ApplyEnd();
            reporter?.Done();
        }

        /// <summary>
        /// Gets called when filter starts applying.
        /// </summary>
        protected virtual void ApplyStart() { }

        /// <summary>
        /// Gets called for each pixel independently.
        /// </summary>
        /// <param name="r">Pointer to red value</param>
        /// <param name="g">Pointer to green value</param>
        /// <param name="b">Pointer to blue value</param>
        protected abstract unsafe void ProcessPixel(byte* r, byte* g, byte* b);

        /// <summary>
        /// Gets called when filter finishes applying.
        /// </summary>
        protected virtual void ApplyEnd() { }
    }
}
