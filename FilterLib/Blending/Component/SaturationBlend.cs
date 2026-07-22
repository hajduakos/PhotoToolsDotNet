using FilterLib.Util;

namespace FilterLib.Blending.Component;

[Blend("Keep bottom hue and lightness, take saturation from top.")]
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
