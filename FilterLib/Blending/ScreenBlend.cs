namespace FilterLib.Blending
{
    /// <summary>
    /// Screen blend mode.
    /// </summary>
    [Blend]
    public sealed class ScreenBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public ScreenBlend(int opacity = 100) : base(opacity) { }

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
            nVal = (255 - *compTop) * *compBottom / 255f + *compTop;
            if (nVal > 255) nVal = 255;
            *compBottom = (byte)(op0 * *compBottom + op1 * nVal);
        }
    }
}
