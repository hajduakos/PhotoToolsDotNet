using FilterLib.Util;

namespace FilterLib.Blending.Inversion;

[Blend("Add layers and subtract double their product.")]
public sealed class ExcludeBlend : PerComponentBlendBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="opacity">Opacity [0:100]</param>
    public ExcludeBlend(int opacity = 100) : base(opacity) { }

    /// <inheritdoc/>
    protected override byte BlendComponent(byte compBottom, byte compTop) =>
        (compTop + compBottom - 2 * compBottom * compTop / 255f).ClampToByte();
}
