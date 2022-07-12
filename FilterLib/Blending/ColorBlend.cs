using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// For each pixel, keep the lightness of the bottom color,
    /// but use the hue and saturation of the top color.
    /// </summary>
    [Blend]
    public sealed class ColorBlend : PerPixelBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public ColorBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe (byte, byte, byte) BlendPixel(byte botR, byte botG, byte botB, byte topR, byte topG, byte topB)
        {
            HSL hslBot = new RGB(botR, botG, botB).ToHSL();
            HSL hslTop = new RGB(topR, topG, topB).ToHSL();
            RGB blended = new HSL(hslTop.H, hslTop.S, hslBot.L).ToRGB();
            return (blended.R, blended.G, blended.B);
        }
    }
}
