namespace FilterLib.Util
{
    /// <summary>
    /// Represents a color in the RGB (red, green, blue) color space.
    /// </summary>
    public struct RGB
    {
        /// <summary> Red component </summary>
        public int R { get; private set; }

        /// <summary> Green component </summary>
        public int G { get; private set; }

        /// <summary> Blue component </summary>
        public int B { get; private set; }

        /// <summary>
        /// Constructor with RGB components.
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        public RGB(int r = 0, int g = 0, int b = 0)
        {
            this.R = r.Clamp(0, 255);
            this.G = g.Clamp(0, 255);
            this.B = b.Clamp(0, 255);
        }

        /// <summary>
        /// Convert to HSL color.
        /// </summary>
        /// <returns>HSL color</returns>
        public HSL ToHSL()
        {
            float h = 0, s = 0, l;
            float rf = R / 255f;
            float gf = G / 255f;
            float bf = B / 255f;
            float min = rf < gf ? rf : gf;
            min = min < bf ? min : bf;
            float max = rf > gf ? rf : gf;
            max = max > bf ? max : bf;
            l = (max + min) / 2;
            if (max != min)
            {
                if (rf == max) h = (gf - bf) / (max - min);
                if (gf == max) h = 2 + (bf - rf) / (max - min);
                if (bf == max) h = 4 + (rf - gf) / (max - min);

                if (l < 0.5) s = (max - min) / (max + min);
                else s = (max - min) / (2f - max - min);
            }
            h *= 60;
            if (h < 0) h += 360;
            s *= 100;
            l *= 100;
            return new HSL((int)h, (int)s, (int)l);
        }

        public override bool Equals(object obj) => obj is RGB && this == (RGB)obj;

        public override int GetHashCode() => R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode();

        public static bool operator ==(RGB c1, RGB c2) => c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;

        public static bool operator !=(RGB c1, RGB c2) => !(c1 == c2);

        public override string ToString() => "(" + R + ", " + G + ", " + B + ")";
    }
}
