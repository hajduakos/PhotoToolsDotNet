namespace FilterLib.Util
{
    /// <summary>
    /// Represent a color in the HSL (hue, saturation, lightness) color space.
    /// </summary>
    public struct HSL : System.IEquatable<HSL>
    {
        /// <summary> Hue </summary>
        public int H { get; private set; }

        /// <summary> Saturation </summary>
        public int S { get; private set; }

        /// <summary> Lightness </summary>
        public int L { get; private set; }

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
                switch (sextant)
                {
                    case 0: r = v; g = mid1; b = m; break;
                    case 1: r = mid2; g = v; b = m; break;
                    case 2: r = m; g = v; b = mid1; break;
                    case 3: r = m; g = mid2; b = v; break;
                    case 4: r = mid1; g = m; b = v; break;
                    case 5: r = v; g = m; b = mid2; break;
                    default: throw new System.Exception($"Sextant {sextant} should be between 0 and 5.");
                }
            }
            return new RGB((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }
        public override bool Equals(object obj) => obj is HSL h && this == h;

        public override int GetHashCode() => H.GetHashCode() ^ S.GetHashCode() ^ L.GetHashCode();

        public static bool operator ==(HSL c1, HSL c2) => c1.H == c2.H && c1.S == c2.S && c1.L == c2.L;

        public static bool operator !=(HSL c1, HSL c2) => !(c1 == c2);

        public override string ToString() => $"HSL({H}, {S}, {L})";

        public bool Equals(HSL other) => this == other;
    }
}
