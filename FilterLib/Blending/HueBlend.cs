using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Keep the saturation and lightness of the bottom layer,
    /// but use the hue of the top layer.
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
