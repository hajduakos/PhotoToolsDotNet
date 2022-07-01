namespace FilterLib.Blending
{
    /// <summary>
    /// Normal blend mode.
    /// </summary>
    [Blend]
    public sealed class NormalBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public NormalBlend(int opacity = 100) : base(opacity) { }

        private float op0, op1;

        /// <inheritdoc/>
        protected override void BlendStart()
        {
            op1 = Opacity / 100.0f;
            op0 = 1 - op1;
        }

        /// <inheritdoc/>
        protected override unsafe void BlendComponent(byte* compBottom, byte* compTop)
        {
            *compBottom = (byte)(op0 * *compBottom + op1 * *compTop);
        }
    }
}
