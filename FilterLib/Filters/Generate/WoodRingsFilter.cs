using FilterLib.Reporting;
using Math = System.Math;
using MathF = System.MathF;

namespace FilterLib.Filters.Generate
{
    /// <summary>
    /// Generate wood rings looking pattern.
    /// </summary>
    [Filter]
    public sealed class WoodRingsFilter : GeneratorBase
    {
        private int rings;
        private float twist;

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
        /// Constructor.
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
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            float sin_mult = MathF.PI * 2 * rings;
            reporter?.Report(0, 0, 2 * image.Height - 1);
            float[,] turb = GenerateTurbulence(image.Width, image.Height);
            reporter?.Report(image.Height, 0, 2 * image.Height - 1);
            fixed (byte* start = image)
            {
                byte* ptr = start;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        float x0 = (x - image.Width / 2) / (float)image.Width;
                        float y0 = (y - image.Height / 2) / (float)image.Height;

                        ptr[0] = ptr[1] = ptr[2] =
                            (byte)(255 * Math.Abs(MathF.Sin(sin_mult * (MathF.Sqrt(x0 * x0 + y0 * y0) + twist * turb[x, y]))));
                        ptr += 3;
                    }
                    reporter?.Report(image.Height + y, 0, 2 * image.Height - 1);
                }
            }
            reporter?.Done();
        }
    }
}
