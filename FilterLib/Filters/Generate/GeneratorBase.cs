using Math = System.Math;

namespace FilterLib.Filters.Generate
{
    /// <summary>
    /// Base class for generators.
    /// </summary>
    public abstract class GeneratorBase : FilterInPlaceBase
    {
        private int iterations;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="iterations">Number of iterations [1;...]</param>
        /// <param name="seed">Random number generator seed</param>
        protected GeneratorBase(int iterations, int seed)
        {
            this.Iterations = iterations;
            this.Seed = seed;
        }

        /// <summary>
        /// Number of iterations [1]
        /// </summary>
        [FilterParam]
        [FilterParamMin(1)]
        public int Iterations
        {
            get { return iterations; }
            set { iterations = Math.Max(1, value); }
        }

        /// <summary>
        /// Random number generator seed
        /// </summary>
        [FilterParam]
        public int Seed { get; set; }

        /// <summary>
        /// Generate a random turbulence map, the values are between 0 and 1.
        /// </summary>
        /// <param name="width">Width of the map</param>
        /// <param name="height">Height of the map</param>
        /// <returns>Random turbulence map</returns>
        protected float[,] GenerateTurbulence(int width, int height)
        {
            int x, y;
            float sum;
            int pow = 1 << iterations;
            int pow0;
            int x0, x1, y0, y1;
            float xFloat, yFloat, xFrac, yFrac;
            float[,] noise = new float[width, height];
            float[,] turbulence = new float[width, height];
            System.Random rnd = new System.Random(Seed);
            for (x = 0; x < width; ++x)
                for (y = 0; y < height; ++y)
                    noise[x, y] = (float)rnd.NextDouble();

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    sum = 0;
                    pow0 = pow;
                    while (pow0 >= 1)
                    {
                        xFloat = x / (float)pow0;
                        yFloat = y / (float)pow0;
                        x0 = (int)Math.Floor(xFloat);
                        x1 = (int)Math.Ceiling(xFloat);
                        xFrac = xFloat - x0;
                        y0 = (int)Math.Floor(yFloat);
                        y1 = (int)Math.Ceiling(yFloat);
                        yFrac = yFloat - y0;

                        sum += pow0 * (
                            noise[x0, y0] * (1 - xFrac) * (1 - yFrac) +
                            noise[x0, y1] * (1 - xFrac) * yFrac +
                            noise[x1, y0] * xFrac * (1 - yFrac) +
                            noise[x1, y1] * xFrac * yFrac
                            );
                        pow0 >>= 1;
                    }
                    turbulence[x, y] = sum / pow * .5f;
                }
            }

            return turbulence;
        }
    }
}
