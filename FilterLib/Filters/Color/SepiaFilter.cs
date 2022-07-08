using FilterLib.Util;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Sepia filter creating an old looking brownish color.
    /// </summary>
    [Filter]
    public sealed class SepiaFilter : PerPixelFilterBase
    {
        /// <inheritdoc/>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            // Calculate sepia values using formula
            float rs = .393f * (*r) + .769f * (*g) + .189f * (*b);
            float gs = .349f * (*r) + .686f * (*g) + .168f * (*b);
            float bs = .272f * (*r) + .534f * (*g) + .131f * (*b);

            *r = rs.ClampToByte();
            *g = gs.ClampToByte();
            *b = bs.ClampToByte();
        }
    }
}
