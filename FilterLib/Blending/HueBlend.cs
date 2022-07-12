using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// For each pixel, keep the saturation and lightness
    /// of the bottom color, but use the hue of the top color.
    /// </summary>
    [Blend]
    public sealed class HueBlend : HSLBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public HueBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override HSL BlendHSL(HSL bot, HSL top) => new(top.H, bot.S, bot.L);
    }
}
