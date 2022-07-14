using FilterLib.Util;

namespace FilterLib.Blending.Component
{
    /// <summary>
    /// Keep the hue and saturation of the bottom layer,
    /// but use the lightness of the top layer.
    /// </summary>
    [Blend]
    public sealed class LightnessBlend : HSLBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public LightnessBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override HSL BlendHSL(HSL bot, HSL top) => new(bot.H, bot.S, top.L);
    }
}
