using FilterLib.Util;

namespace FilterLib.Blending.Cancelation;

[Blend("Subtract top layer from bottom, clamping negatives to black.")]
public sealed class SubtractBlend : PerComponentBlendBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="opacity">Opacity [0:100]</param>
    public SubtractBlend(int opacity = 100) : base(opacity) { }

    /// <inheritdoc/>
    protected override byte BlendComponent(byte compBottom, byte compTop) =>
        (compBottom - compTop).ClampToByte();
}
