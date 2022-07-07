using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Color dodge blend mode.
    /// </summary>
    [Blend]
    public sealed class ColorDodgeBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public ColorDodgeBlend(int opacity = 100) : base(opacity) { }

        private float op0, op1;
        float inv;
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
            inv = 255 - *compTop;
            if (inv == 0) nVal = 255;
            else nVal = (*compBottom / inv * 255f).Clamp(0, 255);
            *compBottom = (byte)(op0 * *compBottom + op1 * nVal);
        }
    }
}
