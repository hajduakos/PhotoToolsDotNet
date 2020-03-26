using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;

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
            this.Rings = rings;
            this.Twist = twist;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new DisposableBitmapData(image, PixelFormat.Format24bppRgb))
            {
                int x, y, xDiv3;
                int w = image.Width;
                int wMul3 = w * 3;
                int h = image.Height;
                float xShifted, yShifted;
                float sin_mult = (float)(Math.PI * 2 * rings);
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
                        for (x = 0; x < wMul3; x += 3)
                        {
                            xDiv3 = x / 3;
                            xShifted = (xDiv3 - w / 2) / (float)w;
                            yShifted = (y - h / 2) / (float)h;

                            row[x] = row[x + 1] = row[x + 2] = (byte)(
                                255 * Math.Abs(Math.Sin(sin_mult * (Math.Sqrt(xShifted * xShifted + yShifted * yShifted) + twist * turbulence[xDiv3, y])
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
