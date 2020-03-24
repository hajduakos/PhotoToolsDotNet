namespace FilterLib.Blending
{
    /// <summary>
    /// Normal blend mode.
    /// </summary>
    public sealed class NormalBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public NormalBlend(int opacity = 100) : base(opacity) { }

        float op0, op1;

        /// <summary>
        /// Gets called when processing starts.
        /// </summary>
        protected override void BlendStart()
        {
            op1 = Opacity / 100.0f;
            op0 = 1 - op1;
        }

        /// <summary>
        /// Blend a component (R/G/B).
        /// </summary>
        /// <param name="compBottom">Bottom component</param>
        /// <param name="compTop">Top component</param>
        protected override unsafe void BlendComponent(byte* compBottom, byte* compTop)
        {
            compBottom[0] = (byte)(op0 * compBottom[0] + op1 * compTop[0]);
        }
    }
}
