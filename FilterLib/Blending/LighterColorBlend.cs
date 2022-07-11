namespace FilterLib.Blending
{
    /// <summary>
    /// For each pixel, pick the lighter color (based on luminance).
    /// </summary>
    [Blend]
    public sealed class LighterColorBlend : PerPixelBlendBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public LighterColorBlend(int opacity = 100) : base(opacity) { }

        private float op0, op1;

        /// <inheritdoc/>
        protected override void BlendStart()
        {
            op1 = Opacity / 100.0f;
            op0 = 1 - op1;
        }

        /// <inheritdoc/>
        protected override unsafe void BlendPixel(byte* botR, byte* botG, byte* botB, byte* topR, byte* topG, byte* topB)
        {
            if (Util.RGB.GetLuminance(*botR, *botG, *botB) < Util.RGB.GetLuminance(*topR, *topG, *topB))
            {
                *botR = (byte)(op0 * (*botR) + op1 * (*topR));
                *botG = (byte)(op0 * (*botG) + op1 * (*topG));
                *botB = (byte)(op0 * (*botB) + op1 * (*topB));
            }
        }
    }
}
