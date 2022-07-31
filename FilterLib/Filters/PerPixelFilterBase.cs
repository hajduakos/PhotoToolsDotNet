using IReporter = FilterLib.Reporting.IReporter;

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
            ApplyStart();
            fixed (byte* start = image)
            {
                byte* ptr = start;
                // Iterate through each pixel and process individually
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        ProcessPixel(ptr, ptr + 1, ptr + 2);
                        ptr += 3;
                    }
                    reporter?.Report(y + 1, 0, image.Height);
                }

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
