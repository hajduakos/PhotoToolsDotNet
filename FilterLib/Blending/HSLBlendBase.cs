using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Base class for blends that operate in the HSL color space.
    /// </summary>
    public abstract class HSLBlendBase : PerPixelBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public HSLBlendBase(int opacity = 100) : base(opacity) { }

        /// <summary>
        /// Blend two pixels in HSL color space.
        /// </summary>
        /// <param name="bot">Bottom pixel</param>
        /// <param name="top">Top pixel</param>
        /// <returns>Blended pixel</returns>
        protected abstract HSL BlendHSL(HSL bot, HSL top);

        /// <inheritdoc/>
        protected override sealed (byte, byte, byte) BlendPixel(byte botR, byte botG, byte botB, byte topR, byte topG, byte topB)
        {
            HSL hslBot = new RGB(botR, botG, botB).ToHSL();
            HSL hslTop = new RGB(topR, topG, topB).ToHSL();
            RGB blended = BlendHSL(hslBot, hslTop).ToRGB();
            return (blended.R, blended.G, blended.B);
        }
    }
}
