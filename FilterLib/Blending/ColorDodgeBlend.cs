using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Color dodge blend mode.
    /// </summary>
    public sealed class ColorDodgeBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public ColorDodgeBlend(int opacity = 100) : base(opacity) { }

        float op0, op1;
        int inv;
        float nVal;

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
            inv = 255 - compTop[0];
            if (inv == 0) nVal = 255;
            else nVal = (compBottom[0] / (float)inv * 255.0f).Clamp(0, 255);
            compBottom[0] = (byte)(op0 * compBottom[0] + op1 * nVal);
        }
    }
}
