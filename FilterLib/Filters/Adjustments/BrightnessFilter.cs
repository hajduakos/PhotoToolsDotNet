using FilterLib.Util;

namespace FilterLib.Filters.Adjustments;

[Filter("Brighten or darken the image by adding a fixed amount to every pixel.")]
public sealed class BrightnessFilter : PerComponentFilterBase
{
    /// <summary>
    /// Brightness adjustment amount [-255;255]
    /// </summary>
    [FilterParam]
    [FilterParamMin(-255)]
    [FilterParamMax(255)]
    public int Brightness
    {
        get ;
        set { field = value.Clamp(-255, 255); }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="brightness">Brightness adjustment amount [-255;255]</param>
    public BrightnessFilter(int brightness = 0) => Brightness = brightness;

    /// <inheritdoc/>
    protected override byte MapComponent(byte comp) => (comp + Brightness).ClampToByte();
}
