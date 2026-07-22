using FilterLib.Util;

namespace FilterLib.Blending.Contrast;

[Blend("Color burn where the top is dark and color dodge where it is light.")]
public sealed class VividLightBlend : PerComponentBlendBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="opacity">Opacity [0:100]</param>
    public VividLightBlend(int opacity = 100) : base(opacity) { }

    /// <inheritdoc/>
    protected override byte BlendComponent(byte compBottom, byte compTop)
    {
        if (compTop > 127)
            return (compBottom / 2f / (1 - compTop / 255f)).ClampToByte();
        else
            return (255 - (255 - compBottom) / (2 * compTop / 255f)).ClampToByte();
    }
}
