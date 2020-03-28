using System;
using FilterLib.Util;

namespace FilterLib.Filters.Noise
{
    /// <summary>
    /// Add noise filter.
    /// </summary>
    [Filter]
    public sealed class AddNoiseFilter : PerPixelFilterBase
    {
        private int intensity, strength;

        /// <summary>
        /// Possible noise types.
        /// </summary>
        public enum NoiseType { Monochrome, Color }

        /// <summary>
        /// Intensity of the noise [0;1000].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(1000)]
        public int Intensity
        {
            get { return intensity; }
            set { intensity = value.Clamp(0, 1000); }
        }

        /// <summary>
        /// Strength of the noise [0;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(255)]
        public int Strength
        {
            get { return strength; }
            set { strength = value.Clamp(0, 255); }
        }

        /// <summary>
        /// Type of noise.
        /// </summary>
        [FilterParam]
        public NoiseType Type { get; set; }

        /// <summary>
        /// Random number generator seed.
        /// </summary>
        [FilterParam]
        public int Seed { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="intensity">Intensity of the noise [0;1000]</param>
        /// <param name="strength">Strength of the noise [0;255]</param>
        /// <param name="type">Type of noise</param>
        /// <param name="seed">Random number generator seed</param>
        public AddNoiseFilter(int intensity = 0, int strength = 0, NoiseType type = NoiseType.Color, int seed = 0)
        {
            Intensity = intensity;
            Strength = strength;
            Type = type;
            Seed = seed;
        }

        int rn, gn, bn;
        System.Random rnd;

        /// <summary>
        /// Gets called when filter starts applying.
        /// </summary>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            rnd = new Random(Seed);
        }

        /// <summary>
        /// Gets called for each pixel independently.
        /// </summary>
        /// <param name="r">Pointer to red value</param>
        /// <param name="g">Pointer to green value</param>
        /// <param name="b">Pointer to blue value</param>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            // Decide to add noise to this pixel or not
            if (rnd.Next(1000) < intensity)
            {
                if (Type == NoiseType.Monochrome) // Monochrome noise -> same noise added to each channel
                {
                    int noise = (int)((rnd.NextDouble() * 2 - 1) * strength);
                    rn = *r + noise;
                    gn = *g + noise;
                    bn = *b + noise;
                }
                else // Color noise -> separate values added to each channel
                {
                    rn = *r + (int)((rnd.NextDouble() * 2 - 1) * strength);
                    gn = *g + (int)((rnd.NextDouble() * 2 - 1) * strength);
                    bn = *b + (int)((rnd.NextDouble() * 2 - 1) * strength);
                }
                // Overwrite old values
                *r = (byte)rn.Clamp(0, 255);
                *g = (byte)gn.Clamp(0, 255);
                *b = (byte)bn.Clamp(0, 255);
            }
        }

        /// <summary>
        /// Gets called when filter finishes applying.
        /// </summary>
        protected override void ApplyEnd()
        {
            rnd = null;
            base.ApplyEnd();
        }
    }
}
