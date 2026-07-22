namespace FilterLib.Filters.Color;

[Filter("Invert every color to its opposite, producing a photo negative.")]
public sealed class InvertFilter : PerComponentFilterBase
{
    /// <inheritdoc/>
    protected override byte MapComponent(byte comp) => (byte)(255 - comp);
}
