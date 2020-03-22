namespace FilterLib.Reporting
{
    /// <summary>
    /// Helper class for creating a recursive sub-reporter
    /// </summary>
    public sealed class SubReporter : IReporter
    {
        private readonly IReporter parent;
        private readonly int pmin, pmax, smin, smax;

        /// <summary>
        /// Create a subreporter for a given parent reporter.
        /// The subreporter will report in the subrange [smin;smax] of the
        /// parent range [pmin;pmax].
        /// </summary>
        /// <param name="parent">Parent reporter</param>
        /// <param name="smin">Start of subrange</param>
        /// <param name="smax">End of subrange</param>
        /// <param name="pmin">Start of parent range</param>
        /// <param name="pmax">End of parent range</param>
        public SubReporter(IReporter parent, int smin = 0, int smax = 100, int pmin = 0, int pmax = 100)
        {
            this.parent = parent;
            this.pmin = pmin;
            this.pmax = pmax;
            this.smin = smin;
            this.smax = smax;
        }
        public void Done() => parent.Report(smax, pmin, pmax);

        public void Report(int value, int min = 0, int max = 100) =>
            parent.Report((int)((value - min) / (float)(max - min) * (smax - smin) + smin), pmin, pmax);

        public void Start() => parent.Report(smin, pmin, pmax);
    }
}
