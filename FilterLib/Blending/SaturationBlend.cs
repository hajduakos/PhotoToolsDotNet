using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Keep the hue and lightness of the bottom layer,
    /// but use the saturation of the top layer.
    /// </summary>
    [Blend]
    public sealed class SaturationBlend : HSLBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public SaturationBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override HSL BlendHSL(HSL bot, HSL top) => new(bot.H, top.S, bot.L);
    }
}
