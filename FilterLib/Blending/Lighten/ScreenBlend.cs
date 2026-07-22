using FilterLib.Util;

namespace FilterLib.Blending.Lighten;

[Blend("Invert both layers, multiply, and invert result.")]
public sealed class ScreenBlend : PerComponentBlendBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="opacity">Opacity [0:100]</param>
    public ScreenBlend(int opacity = 100) : base(opacity) { }

    /// <inheritdoc/>
    protected override byte BlendComponent(byte compBottom, byte compTop) =>
        (255 - (255 - compBottom) * (255 - compTop) / 255f).ClampToByte();
}
