namespace FilterLib.Blending.Lighten;

[Blend("Pick the lighter of the two colors by luminance.")]
public sealed class LighterColorBlend : PerPixelBlendBase
{

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="opacity">Opacity [0:100]</param>
    public LighterColorBlend(int opacity = 100) : base(opacity) { }

    /// <inheritdoc/>
    protected override (byte, byte, byte) BlendPixel(byte botR, byte botG, byte botB, byte topR, byte topG, byte topB)
    {
        if (Util.RGB.GetLuminance(botR, botG, botB) < Util.RGB.GetLuminance(topR, topG, topB))
            return (topR, topG, topB);
        else
            return (botR, botG, botB);
    }
}
