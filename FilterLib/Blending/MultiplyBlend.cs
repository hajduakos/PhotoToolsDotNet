namespace FilterLib.Blending
{
    /// <summary>
    /// Multiply blend mode.
    /// </summary>
    [Blend]
    public sealed class MultiplyBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public MultiplyBlend(int opacity = 100) : base(opacity) { }

        private float op0, op1;
        private float nVal;

        /// <inheritdoc/>
        protected override void BlendStart()
        {
            op1 = Opacity / 100.0f;
            op0 = 1 - op1;
        }

        /// <inheritdoc/>
        protected override unsafe void BlendComponent(byte* compBottom, byte* compTop)
        {
            nVal = *compBottom * *compTop / 255f;
            *compBottom = (byte)(op0 * *compBottom + op1 * nVal);
        }
    }
}
