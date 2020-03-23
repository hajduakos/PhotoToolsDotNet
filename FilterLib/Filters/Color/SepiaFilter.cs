namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Sepia filter.
    /// </summary>
    [Filter]
    public sealed class SepiaFilter : PerPixelFilterBase
    {
        /// <summary>
        /// Gets called for each pixel independently.
        /// </summary>
        /// <param name="r">Pointer to red value</param>
        /// <param name="g">Pointer to green value</param>
        /// <param name="b">Pointer to blue value</param>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            // Calculate sepia values using formula
            int rs = (int)(.393 * (*r) + .769 * (*g) + .189 * (*b));
            int gs = (int)(.349 * (*r) + .686 * (*g) + .168 * (*b));
            int bs = (int)(.272 * (*r) + .534 * (*g) + .131 * (*b));
            if (rs > 255) rs = 255;
            if (gs > 255) gs = 255;
            if (bs > 255) bs = 255;

            *r = (byte)rs;
            *g = (byte)gs;
            *b = (byte)bs;
        }
    }
}
