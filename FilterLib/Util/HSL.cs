namespace FilterLib.Util
{
    /// <summary>
    /// Represent a color in the HSL (hue, saturation, lightness) color space.
    /// </summary>
    public struct HSL
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
            this.H = h;
            while (this.H < 0) this.H += 360;
            this.H %= 360;
            this.S = s.Clamp(0, 100);
            this.L = l.Clamp(0, 100);
        }

        /// <summary>
        /// Convert to RGB color.
        /// </summary>
        /// <returns>RGB color</returns>
        public RGB ToRGB()
        {
            float r, g, b, v;
            float lf = this.L / 100f;
            float sf = this.S / 100f;
            float hf = this.H / 360f;
            r = g = b = lf;
            v = (lf <= 0.5f) ? (lf * (1f + sf)) : (lf + sf - lf * sf);
            if (v > 0)
            {
                float m;
                float sv;
                int sextant;
                float fract, vsf, mid1, mid2;
                m = lf + lf - v;
                sv = (v - m) / v;
                hf *= 6f;
                sextant = (int)hf;
                fract = hf - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0: r = v; g = mid1; b = m; break;
                    case 1: r = mid2; g = v; b = m; break;
                    case 2: r = m; g = v; b = mid1; break;
                    case 3: r = m; g = mid2; b = v; break;
                    case 4: r = mid1; g = m; b = v; break;
                    case 5: r = v; g = m; b = mid2; break;
                }
            }
            return new RGB((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }
        public override bool Equals(object obj) => obj is HSL && this == (HSL)obj;

        public override int GetHashCode() => H.GetHashCode() ^ S.GetHashCode() ^ L.GetHashCode();

        public static bool operator ==(HSL c1, HSL c2) => c1.H == c2.H && c1.S == c2.S && c1.L == c2.L;

        public static bool operator !=(HSL c1, HSL c2) => !(c1 == c2);

        public override string ToString() => $"HSL({H}, {S}, {L})";
    }
}
