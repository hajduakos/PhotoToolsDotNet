using FilterLib.Reporting;
using Math = System.Math;
using MathF = System.MathF;

namespace FilterLib.Filters.Generate
{
    /// <summary>
    /// Generate a marble-like pattern.
    /// </summary>
    [Filter]
    public sealed class MarbleFilter : GeneratorBase
    {
        private int horizLines, vertLines;
        private float twist;

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
        /// Constructor.
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
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            float xMul = horizLines / (float)image.Width;
            float yMul = vertLines / (float)image.Height;
            reporter?.Report(0, 0, 2 * image.Height);
            float[,] turb = GenerateTurbulence(image.Width, image.Height);
            reporter?.Report(image.Height, 0, 2 * image.Height);

            fixed (byte* start = image)
            {
                byte* ptr = start;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        ptr[0] = ptr[1] = ptr[2] =
                            (byte)(255 * Math.Abs(MathF.Sin(MathF.PI * (x * xMul + y * yMul + twist * turb[x, y]))));
                        ptr += 3;
                    }
                    reporter?.Report(image.Height + y + 1, 0, 2 * image.Height);
                }
            }
            reporter?.Done();
        }
    }
}
