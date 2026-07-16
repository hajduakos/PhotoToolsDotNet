namespace FilterLib.Filters.Color
{
    [Filter("Invert the image by inverting all components of all pixels.")]
    public sealed class InvertFilter : PerComponentFilterBase
    {
        /// <inheritdoc/>
        protected override byte MapComponent(byte comp) => (byte)(255 - comp);
    }
}
