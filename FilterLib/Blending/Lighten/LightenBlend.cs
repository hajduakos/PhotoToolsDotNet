namespace FilterLib.Blending.Lighten;

[Blend("Pick the lighter value for each component.")]
public sealed class LightenBlend : PerComponentBlendBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="opacity">Opacity [0:100]</param>
    public LightenBlend(int opacity = 100) : base(opacity) { }

    /// <inheritdoc/>
    protected override byte BlendComponent(byte compBottom, byte compTop) =>
        System.Math.Max(compBottom, compTop);
}
