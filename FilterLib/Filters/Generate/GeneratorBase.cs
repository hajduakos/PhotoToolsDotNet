using Math = System.Math;
using MathF = System.MathF;

namespace FilterLib.Filters.Generate
{
    /// <summary>
    /// Base class for turbulance-based generators.
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
            Iterations = iterations;
            Seed = seed;
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
        /// Generate a random turbulence map with values between 0 and 1.
        /// </summary>
        /// <param name="width">Width of the map</param>
        /// <param name="height">Height of the map</param>
        /// <returns>Random turbulence map</returns>
        protected float[,] GenerateTurbulence(int width, int height)
        {
            int pow = 1 << iterations;
            float[,] noise = new float[width, height];
            float[,] turbulence = new float[width, height];
            System.Random rnd = new(Seed);
            for (int x = 0; x < width; ++x)
                for (int y = 0; y < height; ++y)
                    noise[x, y] = rnd.NextSingle();

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    float sum = 0;
                    int pow0 = pow;
                    while (pow0 >= 1)
                    {
                        float xFloat = x / (float)pow0;
                        float yFloat = y / (float)pow0;
                        int x0 = (int)MathF.Floor(xFloat);
                        int x1 = (int)MathF.Ceiling(xFloat);
                        float xFrac = xFloat - x0;
                        int y0 = (int)MathF.Floor(yFloat);
                        int y1 = (int)MathF.Ceiling(yFloat);
                        float yFrac = yFloat - y0;

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
