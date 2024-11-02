namespace FilterLib.Filters.Edges
{
    /// <summary>
    /// Prewitt filter combining two convolutions.
    /// </summary>
    [Filter]
    public sealed class PrewittFilter : DualConvolutionBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PrewittFilter() : base(
            new(new int[,] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } }, 1, 0),
            new(new int[,] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } }, 1, 0))
        { }
    }
}
