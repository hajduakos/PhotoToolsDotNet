namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Invert the image by inverting all components of all pixels.
    /// </summary>
    [Filter]
    public sealed class InvertFilter : PerComponentFilterBase
    {
        /// <inheritdoc/>
        protected override byte MapComponent(byte comp) => (byte)(255 - comp);
    }
}
