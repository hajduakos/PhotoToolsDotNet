using FilterLib.Util;

namespace FilterLib.Blending.Inversion;

[Blend("Absolute difference of the two layers.")]
public sealed class DifferenceBlend : PerComponentBlendBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="opacity">Opacity [0:100]</param>
    public DifferenceBlend(int opacity = 100) : base(opacity) { }

    /// <inheritdoc/>
    protected override byte BlendComponent(byte compBottom, byte compTop) =>
        System.Math.Abs(compBottom - compTop).ClampToByte();
}
