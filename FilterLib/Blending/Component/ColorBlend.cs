using FilterLib.Util;

namespace FilterLib.Blending.Component;

[Blend("Keep bottom lightness, take hue and saturation from top.")]
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
