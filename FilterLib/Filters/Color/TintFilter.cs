using FilterLib.Util;

namespace FilterLib.Filters.Color;

[Filter("Adjust color tint along the green-magenta axis.")]
public sealed class TintFilter : PerPixelFilterBase
{
    /// <summary>
    /// Tint adjustment [-100;100]. Negative is green, positive is magenta, 0 is neutral.
    /// </summary>
    [FilterParam]
    [FilterParamMin(-100)]
    [FilterParamMax(100)]
    public int Tint
    {
        get;
        set { field = value.Clamp(-100, 100); }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="tint">Tint adjustment [-100;100]</param>
    public TintFilter(int tint = 0)
    {
        Tint = tint;
    }

    // Green channel gain at full tint; red and blue move by the same amount in the
    // opposite direction, so the strongest shift halves/doubles the green channel.
    private const double MaxShift = 0.5;
    // Rec. 709 luminance weights, used to keep neutral grays at constant brightness.
    private const double LumaR = 0.2126, LumaG = 0.7152, LumaB = 0.0722;

    // Caches
    private byte[] redMap;
    private byte[] greenMap;
    private byte[] blueMap;

    /// <inheritdoc/>
    protected override void ApplyStart()
    {
        base.ApplyStart();

        // Move green opposite to red+blue along the green-magenta axis: positive tint
        // removes green (magenta), negative tint adds green.
        double t = Tint / 100.0;
        double gainG = 1.0 - MaxShift * t;
        double gainRB = 1.0 + MaxShift * t;

        // Luminance-normalize so a neutral gray only shifts hue, not brightness.
        double luma = (LumaR + LumaB) * gainRB + LumaG * gainG;
        gainG /= luma;
        gainRB /= luma;

        redMap = SRGB.GainMap(gainRB);
        greenMap = SRGB.GainMap(gainG);
        blueMap = SRGB.GainMap(gainRB);
    }

    /// <inheritdoc/>
    protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
    {
        System.Diagnostics.Debug.Assert(redMap != null);
        System.Diagnostics.Debug.Assert(greenMap != null);
        System.Diagnostics.Debug.Assert(blueMap != null);
        // Just use cache
        *r = redMap[*r];
        *g = greenMap[*g];
        *b = blueMap[*b];
    }

    /// <inheritdoc/>
    protected override void ApplyEnd()
    {
        redMap = greenMap = blueMap = null;
        base.ApplyEnd();
    }
}
