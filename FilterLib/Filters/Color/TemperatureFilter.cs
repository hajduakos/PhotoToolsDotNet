using FilterLib.Util;
using System;

namespace FilterLib.Filters.Color;

[Filter("Adjust color temperature along the blue-amber (cold-warm) axis.")]
public sealed class TemperatureFilter : PerPixelFilterBase
{
    /// <summary>
    /// Color temperature in Kelvin [2000;15000]. Higher is warmer, 6500 (D65) is neutral.
    /// </summary>
    [FilterParam]
    [FilterParamMin(2000)]
    [FilterParamMax(15000)]
    public int Temperature
    {
        get;
        set { field = value.Clamp(2000, 15000); }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="temperature">Color temperature in Kelvin [2000;15000]</param>
    public TemperatureFilter(int temperature = 6500)
    {
        Temperature = temperature;
    }

    // Reference white the sRGB image is encoded against (D65).
    private const int ReferenceKelvin = 6500;

    // Caches
    private byte[] redMap;
    private byte[] greenMap;
    private byte[] blueMap;

    /// <inheritdoc/>
    protected override void ApplyStart()
    {
        base.ApplyStart();

        // Von Kries adaptation from the target illuminant to D65: scaling each linear
        // channel by refWhite/targetWhite neutralizes light of the given temperature.
        // Dividing (rather than multiplying) inverts the axis so higher Kelvin is warmer.
        (double tR, double tG, double tB) = LinearWhitePoint(Temperature);
        (double rR, double rG, double rB) = LinearWhitePoint(ReferenceKelvin);
        double gainR = rR / tR;
        double gainG = rG / tG;
        double gainB = rB / tB;

        // Normalize by green to keep the green channel (and overall brightness) fixed;
        // this also makes Temperature == 6500 an identity transform.
        gainR /= gainG;
        gainB /= gainG;
        gainG = 1.0;

        redMap = BuildMap(gainR);
        greenMap = BuildMap(gainG);
        blueMap = BuildMap(gainB);
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

    /// <summary>
    /// Build a lookup table applying a per-channel gain in linear light, so channel
    /// scaling is done on decoded values rather than gamma-encoded ones.
    /// </summary>
    private static byte[] BuildMap(double gain)
    {
        byte[] map = new byte[256];
        for (int x = 0; x < 256; ++x)
        {
            double lin = SrgbDecode(x / 255.0) * gain;
            map[x] = ((float)(SrgbEncode(lin.Clamp(0.0, 1.0)) * 255.0)).ClampToByte();
        }
        return map;
    }

    /// <summary>
    /// Convert a color temperature to its white point in linear sRGB.
    /// </summary>
    private static (double R, double G, double B) LinearWhitePoint(int kelvin)
    {
        (double x, double y) = PlanckianLocus(kelvin);
        // Chromaticity to XYZ at unit luminance (Y = 1).
        double X = x / y;
        double Y = 1.0;
        double Z = (1.0 - x - y) / y;
        // XYZ to linear sRGB matrix (D65).
        double r = 3.2404542 * X - 1.5371385 * Y - 0.4985314 * Z;
        double g = -0.9692660 * X + 1.8760108 * Y + 0.0415560 * Z;
        double b = 0.0556434 * X - 0.2040259 * Y + 1.0572252 * Z;
        return (r, g, b);
    }

    /// <summary>
    /// CIE Planckian-locus approximation: map a blackbody temperature (Kelvin) to a
    /// chromaticity (x, y). Valid roughly for 1667K-25000K.
    /// </summary>
    private static (double x, double y) PlanckianLocus(int kelvin)
    {
        double t = kelvin;
        double x = kelvin <= 4000
            ? -0.2661239e9 / (t * t * t) - 0.2343589e6 / (t * t) + 0.8776956e3 / t + 0.179910
            : -3.0258469e9 / (t * t * t) + 2.1070379e6 / (t * t) + 0.2226347e3 / t + 0.240390;
        double y = kelvin <= 2222
            ? -1.1063814 * x * x * x - 1.34811020 * x * x + 2.18555832 * x - 0.20219683
            : kelvin <= 4000
                ? -0.9549476 * x * x * x - 1.37418593 * x * x + 2.09137015 * x - 0.16748867
                : 3.0817580 * x * x * x - 5.87338670 * x * x + 3.75112997 * x - 0.37001483;
        return (x, y);
    }

    // sRGB gamma decode (companding): encoded [0;1] to linear [0;1].
    private static double SrgbDecode(double u) =>
        u <= 0.04045 ? u / 12.92 : Math.Pow((u + 0.055) / 1.055, 2.4);

    // sRGB gamma encode (companding): linear [0;1] to encoded [0;1].
    private static double SrgbEncode(double u) =>
        u <= 0.0031308 ? 12.92 * u : 1.055 * Math.Pow(u, 1.0 / 2.4) - 0.055;
}
