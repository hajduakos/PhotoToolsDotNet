namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Invert filter.
    /// </summary>
    [Filter]
    public sealed class InvertFilter : PerComponentFilterBase
    {
        /// <inheritdoc/>
        protected override byte MapComponent(byte comp) => (byte)(255 - comp);
    }
}
