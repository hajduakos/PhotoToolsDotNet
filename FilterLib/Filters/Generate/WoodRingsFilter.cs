using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;
using MathF = System.MathF;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

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
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                int x, y, xDiv3;
                int w = image.Width;
                int width_3 = w * 3;
                int h = image.Height;
                float xShifted, yShifted;
                float sin_mult = (MathF.PI * 2 * rings);
                reporter?.Report(0, 0, 2 * h - 1);
                float[,] turbulence = GenerateTurbulence(w, h);
                reporter?.Report(h, 0, 2 * h - 1);

                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            xDiv3 = x / 3;
                            xShifted = (xDiv3 - w / 2) / (float)w;
                            yShifted = (y - h / 2) / (float)h;

                            row[x] = row[x + 1] = row[x + 2] = (byte)(
                                255 * Math.Abs(MathF.Sin(sin_mult * (MathF.Sqrt(xShifted * xShifted + yShifted * yShifted) + twist * turbulence[xDiv3, y])
                                )));
                        }
                        if ((y & 63) == 0) reporter?.Report(h + y, 0, 2 * h - 1);
                    }
                }

            }
            reporter?.Done();
        }
    }
}
