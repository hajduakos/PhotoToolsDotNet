using FilterLib.Util;
using System.IO;
using MathF = System.MathF;

namespace FilterLib.Filters.Color;

[Filter("Flatten each channel to a few levels for a poster-like look.")]
public sealed class PosterizeFilter : PerComponentFilterBase
{
    private float div;

    /// <summary>
    /// Number of levels [2:256].
    /// </summary>
    [FilterParam]
    [FilterParamMin(2)]
    [FilterParamMax(256)]
    public int Levels
    {
        get;
        set
        {
            field = value.Clamp(2, 256);
            div = 255f / (field - 1);
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="levels">Number of levels [2:256]</param>
    public PosterizeFilter(int levels = 256) => Levels = levels;

    /// <inheritdoc/>
    protected override byte MapComponent(byte comp) => (byte)MathF.Round(MathF.Round(comp / div) * div);
}
