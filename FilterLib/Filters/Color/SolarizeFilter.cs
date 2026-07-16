namespace FilterLib.Filters.Color;

[Filter("Leave darker tones as is and invert brighter tones.")]
public sealed class SolarizeFilter : PerComponentFilterBase
{
    /// <inheritdoc/>
    protected override byte MapComponent(byte comp) => (byte)(comp > 127 ? 255 - comp : comp);
}
