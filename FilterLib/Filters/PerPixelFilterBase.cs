using IReporter = FilterLib.Reporting.IReporter;

namespace FilterLib.Filters
{
    /// <summary>
    /// Base class for filters that process each pixel individually.
    /// </summary>
    public abstract class PerPixelFilterBase : FilterInPlaceBase
    {
        /// <inheritdoc/>
        public override sealed void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            ApplyStart();
            unsafe
            {
                fixed (byte* start = image)
                {
                    // Iterate through each pixel and process individually
                    int width_3 = image.Width * 3;
                    for (int y = 0; y < image.Height; ++y)
                    {
                        byte* row = start + y * width_3;
                        for (int x = 0; x < width_3; x += 3)
                        {
                            ProcessPixel(row + x, row + x + 1, row + x + 2);
                        }
                        reporter?.Report(y, 0, image.Height - 1);
                    }

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
