using FilterLib.Util;

namespace FilterLib.Filters.Color;

[Filter("Convert to pure black and white using a single fixed brightness cutoff.")]
public sealed class ThresholdFilter : PerPixelFilterBase
{
    /// <summary>
    /// Threshold value [0;255].
    /// </summary>
    [FilterParam]
    [FilterParamMin(0)]
    [FilterParamMax(255)]
    public int Threshold
    {
        get;
        set { field = value.ClampToByte(); }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="threshold">Threshold value [0:255]</param>
    public ThresholdFilter(int threshold = 127) => Threshold = threshold;

    // Cache
    private byte[] map;

    /// <inheritdoc/>
    protected override void ApplyStart()
    {
        base.ApplyStart();
        // Fill cache
        map = new byte[256];
        for (int i = 0; i < 256; ++i) map[i] = (byte)(i < Threshold ? 0 : 255);
    }

    /// <inheritdoc/>
    protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
    {
        // Use cache
        System.Diagnostics.Debug.Assert(map != null);
        System.Diagnostics.Debug.Assert(map.Length == 256);
        *r = *g = *b = map[(byte)RGB.GetLuminance(*r, *g, *b)];
    }

    /// <inheritdoc/>
    protected override void ApplyEnd()
    {
        map = null;
        base.ApplyEnd();
    }
}
