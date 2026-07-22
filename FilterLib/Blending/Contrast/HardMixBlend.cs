namespace FilterLib.Blending.Contrast;

[Blend("Push each component to fully dark or bright based on bottom and top.")]
public sealed class HardMixBlend : PerComponentBlendBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="opacity">Opacity [0:100]</param>
    public HardMixBlend(int opacity = 100) : base(opacity) { }

    /// <inheritdoc/>
    protected override byte BlendComponent(byte compBottom, byte compTop) =>
        (byte)(compTop < 255 - compBottom ? 0 : 255);

}
