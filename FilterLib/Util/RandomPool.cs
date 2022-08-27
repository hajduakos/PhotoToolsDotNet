using System;

namespace FilterLib.Util
{
    /// <summary>
    /// Represents a pool of random number generators with a given size and initial seed.
    /// </summary>
    public sealed class RandomPool
    {
        private readonly Random[] rnds;

        /// <summary>
        /// Get the ith random number generator.
        /// </summary>
        /// <param name="i">Index</param>
        /// <returns>Random number generator</returns>
        public Random this[int i] { get { return rnds[i]; } }

        /// <summary>
        /// Create a new pool with given size and seed.
        /// </summary>
        /// <param name="count">Size</param>
        /// <param name="seed">Seed</param>
        public RandomPool(int count, int seed)
        {
            rnds = new Random[count];
            Random rnd0 = new(seed);
            for (int i = 0; i < rnds.Length; i++) rnds[i] = new Random(rnd0.Next());
        }
    }
}
