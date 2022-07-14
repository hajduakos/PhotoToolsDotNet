using FilterLib.Util;

namespace FilterLib.Blending.Component
{
    /// <summary>
    /// Keep the lightness of the bottom layer, but use the hue and saturation
    /// of the top layer.
    /// </summary>
    [Blend]
    public sealed class ColorBlend : HSLBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public ColorBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override HSL BlendHSL(HSL bot, HSL top) => new(top.H, top.S, bot.L);
    }
}
