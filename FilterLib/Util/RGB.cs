namespace FilterLib.Util
{
    /// <summary>
    /// Represents a color in the RGB (red, green, blue) color space.
    /// </summary>
    public struct RGB
    {
        /// <summary> Red component </summary>
        public byte R { get; private set; }

        /// <summary> Green component </summary>
        public byte G { get; private set; }

        /// <summary> Blue component </summary>
        public byte B { get; private set; }

        /// <summary>
        /// Constructor with RGB components.
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        public RGB(int r = 0, int g = 0, int b = 0)
        {
            R = r.ClampToByte();
            G = g.ClampToByte();
            B = b.ClampToByte();
        }

        /// <summary>
        /// Constructor from string of format RGB(r, g, b) or (r, g, b).
        /// </summary>
        /// <param name="str">String to be parsed</param>
        public RGB(string str)
        {
            str = str.Replace("RGB(", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            string[] tokens = str.Split(',');
            if (tokens.Length != 3) throw new System.ArgumentException("Expected three comma separated values for RGB.");
            R = int.Parse(tokens[0]).ClampToByte();
            G = int.Parse(tokens[1]).ClampToByte();
            B = int.Parse(tokens[2]).ClampToByte();
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

        public override bool Equals(object obj) => obj is RGB r && this == r;

        public override int GetHashCode() => (R << 16) | (G << 8) | B;

        public static bool operator ==(RGB c1, RGB c2) => c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;

        public static bool operator !=(RGB c1, RGB c2) => !(c1 == c2);

        public override string ToString() => $"RGB({R}, {G}, {B})";
    }
}
