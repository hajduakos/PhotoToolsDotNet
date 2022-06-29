using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FilterLib.Util
{
    /// <summary>
    /// Linear gradient with multiple stops.
    /// </summary>
    public sealed class Gradient
    {
        private readonly List<RGB> colors;
        private readonly List<float> stops;

        /// <summary>
        /// Create a gradient with a list of colors and stops.
        /// The number of colors and stops must be equal. Stops
        /// will be normalized to 0...1.
        /// </summary>
        /// <param name="colors">Colors</param>
        /// <param name="stops">Stops</param>
        public Gradient(IEnumerable<RGB> colors, IEnumerable<float> stops)
        {
            this.colors = new List<RGB>(colors);
            this.stops = new List<float>(stops);
            CheckAndNormalize();
        }

        /// <summary>
        /// Create a uniform linear gradient between two colors.
        /// </summary>
        /// <param name="start">Start color</param>
        /// <param name="end">End color</param>
        public Gradient(RGB start, RGB end) : this(new RGB[] { start, end }, new float[] { 0, 1 }) { }

        /// <summary>
        /// Create a uniform gradient between two colors and a midpoint.
        /// </summary>
        /// <param name="start">Start color</param>
        /// <param name="mid">Midpoint color</param>
        /// <param name="end">End color</param>
        public Gradient(RGB start, RGB mid, RGB end) : this(new RGB[] { start, mid, end }, new float[] { 0, 0.5f, 1 }) { }

        /// <summary>
        /// Parse a gradient from a string of form s (r g b), s (r g b), ..., s (r g b).
        /// </summary>
        /// <param name="str">String to be parsed</param>
        public Gradient(string str)
        {
            colors = new List<RGB>();
            stops = new List<float>();
            str = Regex.Replace(str, @"\s+", " ").Trim();
            string[] tokens = str.Split(',');
            if (tokens.Length < 2) throw new FormatException("Expected at least two components.");
            foreach(string comp in tokens)
            {
                string[] compTokens = comp.Trim().Replace("(", "").Replace(")", "").Split(' ');
                if (compTokens.Length != 4)
                    throw new FormatException("Expected exactly four numbers 'stop (r g b)' per component.");
                stops.Add(float.Parse(compTokens[0], CultureInfo.InvariantCulture.NumberFormat));
                colors.Add(new RGB(int.Parse(compTokens[1]), int.Parse(compTokens[2]), int.Parse(compTokens[3])));
            }
            CheckAndNormalize();
        }

        private void CheckAndNormalize()
        {
            if (this.colors.Count != this.stops.Count)
                throw new ArgumentException("Length of colors and stops must be equal.");
            if (this.colors.Count < 2)
                throw new ArgumentException("At least two colors are required.");
            for (int i = 0; i < this.stops.Count - 1; ++i)
                if (this.stops[i] >= this.stops[i + 1])
                    throw new ArgumentException("Stops must be increasing.");
            // Normalize
            float first = this.stops[0];
            for (int i = 0; i < this.stops.Count; ++i)
                this.stops[i] -= first;
            float last = this.stops[^1];
            for (int i = 0; i < this.stops.Count; ++i)
                this.stops[i] /= last;
        }

        /// <summary>
        /// Create a map of 256 colors for the range of the gradient.
        /// </summary>
        /// <returns>Map of colors</returns>
        public RGB[] CreateMap256()
        {
            RGB[] map = new RGB[256];
            for (int i = 0; i < 256; i++)
                map[i] = GetColor(i / 255f);
            return map;
        }

        /// <summary>
        /// Get a color at a given value.
        /// </summary>
        /// <param name="at">Value, must be between 0 and 1</param>
        /// <returns>Color at the value</returns>
        public RGB GetColor(float at)
        {
            if (at < 0 || at > 1) throw new ArgumentOutOfRangeException(nameof(at), "A value between 0 and 1 is required");
            int gid = 0;
            while (at > stops[gid + 1] + 0.001f) gid++;
            float ratio = (at - stops[gid]) / (stops[gid + 1] - stops[gid]);
            return new RGB(
                (byte)(colors[gid].R + (colors[gid + 1].R - colors[gid].R) * ratio),
                (byte)(colors[gid].G + (colors[gid + 1].G - colors[gid].G) * ratio),
                (byte)(colors[gid].B + (colors[gid + 1].B - colors[gid].B) * ratio));
        }

        public override string ToString()
        {
            List<string> stopsStr = new();
            for (int i = 0; i < stops.Count; i++)
                stopsStr.Add($"{stops[i]} {colors[i]}");
            return $"Gradient({string.Join(", ", stopsStr)})";
        }
    }
}
