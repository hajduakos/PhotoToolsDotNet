namespace FilterLib.Filters.Edges
{
    /// <summary>
    /// Sobel filter combining two convolutions.
    /// </summary>
    [Filter]
    public sealed class SobelFilter : DualConvolutionBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SobelFilter() : base(
            new(-1, -2, -1, 0, 0, 0, 1, 2, 1),
            new(-1, 0, 1, -2, 0, 2, -1, 0, 1))
        { }
    }
}
