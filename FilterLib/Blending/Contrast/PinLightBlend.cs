using FilterLib.Util;
using Math = System.Math;

namespace FilterLib.Blending.Contrast;

[Blend("Darken where the top is dark and lighten where it is light.")]
public sealed class PinLightBlend : PerComponentBlendBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="opacity">Opacity [0:100]</param>
    public PinLightBlend(int opacity = 100) : base(opacity) { }

    /// <inheritdoc/>
    protected override byte BlendComponent(byte compBottom, byte compTop)
    {
        if (compTop > 127) return Math.Max(compBottom, 2 * compTop - 255).ClampToByte();
        else return Math.Min(compBottom, 2 * compTop).ClampToByte();
    }
}
