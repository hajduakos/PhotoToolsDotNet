namespace FilterLib.Filters.Edges
{
    /// <summary>
    /// SCharr filter combining two convolutions.
    /// </summary>
    [Filter]
    public class ScharrFilter : DualConvolutionBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ScharrFilter() : base(
            new(new int[,] { { 3, 10, 3 }, { 0, 0, 0 }, { -3, -10, -3 } }),
            new(new int[,] { { 3, 0, -3 }, { 10, 0, -10 }, { 3, 0, -3 } }))
        { }
    }
}
