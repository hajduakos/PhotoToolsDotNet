using System;

namespace FilterLib.Util;

/// <summary>
/// Utility class for sRGB gamma companding and linear-light channel scaling.
/// </summary>
public static class SRGB
{
    /// <summary>
    /// sRGB gamma decode (companding): encoded [0;1] to linear [0;1].
    /// </summary>
    public static double Decode(double u) =>
        u <= 0.04045 ? u / 12.92 : Math.Pow((u + 0.055) / 1.055, 2.4);

    /// <summary>
    /// sRGB gamma encode (companding): linear [0;1] to encoded [0;1].
    /// </summary>
    public static double Encode(double u) =>
        u <= 0.0031308 ? 12.92 * u : 1.055 * Math.Pow(u, 1.0 / 2.4) - 0.055;

    /// <summary>
    /// Build a 256-entry lookup table applying a per-channel gain in linear light, so
    /// scaling is done on decoded values rather than gamma-encoded ones.
    /// </summary>
    /// <param name="gain">Multiplier applied to the linear value</param>
    /// <returns>Lookup table mapping each input byte to its scaled output</returns>
    public static byte[] GainMap(double gain)
    {
        byte[] map = new byte[256];
        for (int x = 0; x < 256; ++x)
        {
            double lin = Decode(x / 255.0) * gain;
            map[x] = ((float)(Encode(lin.Clamp(0.0, 1.0)) * 255.0)).ClampToByte();
        }
        return map;
    }
}
