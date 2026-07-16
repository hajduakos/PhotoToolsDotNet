namespace FilterLib.Util;

/// <summary>
/// Represent a color in the HSL (hue, saturation, lightness) color space.
/// </summary>
public readonly record struct HSL
{
    /// <summary> Hue </summary>
    public int H { get; }

    /// <summary> Saturation </summary>
    public int S { get; }

    /// <summary> Lightness </summary>
    public int L { get; }

    /// <summary>
    /// Constructor with HSL values.
    /// </summary>
    /// <param name="h">Hue</param>
    /// <param name="s">Saturation</param>
    /// <param name="l">Lightness</param>
    public HSL(int h = 0, int s = 0, int l = 0)
    {
        H = h;
        while (H < 0) H += 360;
        H %= 360;
        S = s.Clamp(0, 100);
        L = l.Clamp(0, 100);
    }

    /// <summary>
    /// Convert to RGB color.
    /// </summary>
    /// <returns>RGB color</returns>
    public RGB ToRGB()
    {
        float r, g, b, v;
        float lf = L / 100f;
        float sf = S / 100f;
        float hf = H / 360f;
        r = g = b = lf;
        v = (lf <= 0.5f) ? (lf * (1f + sf)) : (lf + sf - lf * sf);
        if (v > 0)
        {
            float m = lf + lf - v;
            float sv = (v - m) / v;
            hf *= 6f;
            int sextant = (int)hf;
            float fract = hf - sextant;
            float vsf = v * sv * fract;
            float mid1 = m + vsf;
            float mid2 = v - vsf;
            (r, g, b) = sextant switch
            {
                0 => (v, mid1, m),
                1 => (mid2, v, m),
                2 => (m, v, mid1),
                3 => (m, mid2, v),
                4 => (mid1, m, v),
                5 => (v, m, mid2),
                _ => throw new System.InvalidOperationException($"Sextant {sextant} should be between 0 and 5."),
            };
        }
        return new RGB((int)(r * 255), (int)(g * 255), (int)(b * 255));
    }

    public override string ToString() => $"HSL({H}, {S}, {L})";
}
