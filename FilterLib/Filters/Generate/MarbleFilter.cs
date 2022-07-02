using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;
using MathF = System.MathF;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Generate
{
    /// <summary>
    /// Marble generating filter.
    /// </summary>
    [Filter]
    public sealed class MarbleFilter : GeneratorBase
    {
        private int horizLines, vertLines; // Number of horizontal/vertical lines
        private float twist; // Twist factor

        /// <summary>
        /// Number of horizontal lines.
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int HorizontalLines
        {
            get { return horizLines; }
            set { horizLines = Math.Max(0, value); }
        }

        /// <summary>
        /// Number of vertical lines.
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int VerticalLines
        {
            get { return vertLines; }
            set { vertLines = Math.Max(0, value); }
        }

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
        /// Constructor with iterations and random number seed
        /// </summary>
        /// <param name="horizLines">Number of horizontal lines [0;...]</param>
        /// <param name="vertLines">Number of vertical lines [0;...]</param>
        /// <param name="twist">Twist factor [0;...]</param>
        /// <param name="iterations">Number of iterations [1;...]</param>
        /// <param name="seed">Random number generator seed</param>
        public MarbleFilter(int horizLines = 0, int vertLines = 0, float twist = 0, int iterations = 1, int seed = 0)
            : base(iterations, seed)
        {
            HorizontalLines = horizLines;
            VerticalLines = vertLines;
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
                float xMultiplier = horizLines / (float)w;
                float yMultiplier = vertLines / (float)h;
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
                            row[x] = row[x + 1] = row[x + 2] =
                                (byte)(255 * Math.Abs(MathF.Sin(MathF.PI * (xDiv3 * xMultiplier + y * yMultiplier + twist * turbulence[xDiv3, y]))));
                        }
                        if ((y & 63) == 0) reporter?.Report(h + y, 0, 2 * h - 1);
                    }
                }

            }
            reporter?.Done();
        }
    }
}
