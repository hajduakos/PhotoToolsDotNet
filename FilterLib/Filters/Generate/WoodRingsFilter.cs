using FilterLib.Reporting;
using Math = System.Math;
using MathF = System.MathF;

namespace FilterLib.Filters.Generate
{
    /// <summary>
    /// Wood rings generating filter.
    /// </summary>
    [Filter]
    public sealed class WoodRingsFilter : GeneratorBase
    {
        private int rings; // Number of rings
        private float twist; // Twist factor

        /// <summary>
        /// Twist factor.
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        public float Twist
        {
            get { return twist; }
            set { twist = Math.Max(0, value); }
        }

        /// <summary>
        /// Number of rings.
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int Rings
        {
            get { return rings; }
            set { rings = Math.Max(0, value); }
        }

        /// <summary>
        /// Constructor with iterations and random number seed
        /// </summary>
        /// <param name="rings">Number of rings [0;...]</param>
        /// <param name="twist">Twist factor [0;...]</param>
        /// <param name="iterations">Number of iterations [1;...]</param>
        /// <param name="seed">Random number generator seed</param>
        public WoodRingsFilter(int rings = 0, float twist = 0, int iterations = 1, int seed = 0)
            : base(iterations, seed)
        {
            Rings = rings;
            Twist = twist;
        }

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            unsafe
            {
                fixed (byte* start = image)
                {
                    int x, y, x_div3;
                    int w = image.Width;
                    int width_3 = w * 3;
                    int h = image.Height;
                    float xShifted, yShifted;
                    float sin_mult = (MathF.PI * 2 * rings);
                    reporter?.Report(0, 0, 2 * h - 1);
                    float[,] turbulence = GenerateTurbulence(w, h);
                    reporter?.Report(h, 0, 2 * h - 1);

                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get row
                        byte* row = start + y * width_3;
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            x_div3 = x / 3;
                            xShifted = (x_div3 - w / 2) / (float)w;
                            yShifted = (y - h / 2) / (float)h;

                            row[x] = row[x + 1] = row[x + 2] = (byte)(
                                255 * Math.Abs(MathF.Sin(sin_mult * (MathF.Sqrt(xShifted * xShifted + yShifted * yShifted) + twist * turbulence[x_div3, y])
                                )));
                        }
                        reporter?.Report(h + y, 0, 2 * h - 1);
                    }
                }

            }
            reporter?.Done();
        }
    }
}
