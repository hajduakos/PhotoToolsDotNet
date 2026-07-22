using FilterLib.Util;

namespace FilterLib.Blending.Component;

[Blend("Keep bottom hue and saturation, take lightness from top.")]
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
