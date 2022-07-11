using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// For each pixel, keep the hue and saturation of the bottom color,
    /// but use the lightness of the top color.
    /// </summary>
    [Blend]
    public sealed class LightnessBlend : PerPixelBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public LightnessBlend(int opacity = 100) : base(opacity) { }

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
            HSL hslBot = new RGB(*botR, *botG, *botB).ToHSL();
            HSL hslTop = new RGB(*topR, *topG, *topB).ToHSL();
            RGB blended = new HSL(hslBot.H, hslBot.S, hslTop.L).ToRGB();
            *botR = (byte)(op0 * (*botR) + op1 * blended.R);
            *botG = (byte)(op0 * (*botG) + op1 * blended.G);
            *botB = (byte)(op0 * (*botB) + op1 * blended.B);
        }
    }
}
