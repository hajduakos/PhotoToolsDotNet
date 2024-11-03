namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Cluster dot dither is an ordered dither with a specifically defined matrix
    /// </summary>
    [Filter]
    public class ClusterDotDitherFilter : OrderedDitherFilterBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        public ClusterDotDitherFilter(int levels = 256) : base(levels, new ClusterDotDitherMatrix()) { }
    }
}
