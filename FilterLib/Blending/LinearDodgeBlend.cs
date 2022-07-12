using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Linear dodge blend mode.
    /// </summary>
    [Blend]
    public sealed class LinearDodgeBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public LinearDodgeBlend(int opacity = 100) : base(opacity) { }

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
            nVal = (*compBottom + *compTop).Clamp(0, 255);
            *compBottom = (byte)(op0 * *compBottom + op1 * nVal);
        }
    }
}
