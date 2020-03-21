namespace FilterLib.Color
{
    /// <summary>
    /// Invert filter.
    /// </summary>
    public sealed class InvertFilter : PerComponentFilterBase
    {
        /// <summary>
        /// Map a single (R/G/B) component.
        /// </summary>
        /// <param name="comp">Input value</param>
        /// <returns>Output value by applying the filter</returns>
        protected override byte MapComponent(byte comp) => (byte)(255 - comp);
    }
}
