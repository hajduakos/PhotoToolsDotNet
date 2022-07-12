using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// For each pixel, keep the hue and lightness of the bottom color,
    /// but use the saturation of the top color.
    /// </summary>
    [Blend]
    public sealed class SaturationBlend : PerPixelBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public SaturationBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe (byte, byte, byte) BlendPixel(byte botR, byte botG, byte botB, byte topR, byte topG, byte topB)
        {
            HSL hslBot = new RGB(botR, botG, botB).ToHSL();
            HSL hslTop = new RGB(topR, topG, topB).ToHSL();
            RGB blended = new HSL(hslBot.H, hslTop.S, hslBot.L).ToRGB();
            return (blended.R, blended.G, blended.B);
        }
    }
}
