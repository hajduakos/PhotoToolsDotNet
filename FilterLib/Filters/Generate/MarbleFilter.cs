using FilterLib.Reporting;
using Math = System.Math;
using MathF = System.MathF;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Generate
{
    /// <summary>
    /// Generate a marble-like pattern.
    /// </summary>
    [Filter]
    public sealed class MarbleFilter : GeneratorBase
    {
        /// <summary>
        /// Number of horizontal lines.
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int HorizontalLines
        {
            get;
            set { field = Math.Max(0, value); }
        }

        /// <summary>
        /// Number of vertical lines.
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int VerticalLines
        {
            get;
            set { field = Math.Max(0, value); }
        }

        /// <summary>
        /// Twist factor.
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        public float Twist
        {
            get;
            set { field = Math.Max(0, value); }
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
            object reporterLock = new();
            int progress = 0;
            float xMul = HorizontalLines / (float)image.Width;
            float yMul = VerticalLines / (float)image.Height;
            reporter?.Report(0, 0, 2 * image.Height);
            float[,] turb = GenerateTurbulence(image.Width, image.Height);
            reporter?.Report(image.Height, 0, 2 * image.Height);

            fixed (byte* start = image)
            {
                byte* start0 = start;
                Parallel.For(0, image.Height, y =>
                {
                    byte* ptr = start0 + y * image.Width * 3;
                    for (int x = 0; x < image.Width; ++x)
                    {
                        ptr[0] = ptr[1] = ptr[2] =
                            (byte)(255 * Math.Abs(MathF.Sin(MathF.PI * (x * xMul + y * yMul + Twist * turb[x, y]))));
                        ptr += 3;
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(image.Height + ++progress, 0, 2 * image.Height);
                });
            }
            reporter?.Done();
        }
    }
}
