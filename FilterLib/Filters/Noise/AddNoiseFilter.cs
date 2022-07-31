using FilterLib.Reporting;
using FilterLib.Util;
using Random = System.Random;

namespace FilterLib.Filters.Noise
{
    /// <summary>
    /// Add noise filter.
    /// </summary>
    [Filter]
    public sealed class AddNoiseFilter : FilterInPlaceBase
    {
        private int intensity;
        private int strength;

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
            set { strength = value.ClampToByte(); }
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
        /// Constructor.
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

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            Random rnd = new(Seed);
            int rn, gn, bn;
            fixed (byte* start = image)
            {
                byte* ptr = start;
                // Iterate through each pixel and process individually
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        // Decide to add noise to this pixel or not
                        if (rnd.Next(1000) < intensity)
                        {
                            if (Type == NoiseType.Monochrome) // Monochrome noise -> same noise added to each channel
                            {
                                int noise = (int)((rnd.NextSingle() * 2 - 1) * strength);
                                rn = ptr[0] + noise;
                                gn = ptr[1] + noise;
                                bn = ptr[2] + noise;
                            }
                            else // Color noise -> separate values added to each channel
                            {
                                rn = ptr[0] + (int)((rnd.NextSingle() * 2 - 1) * strength);
                                gn = ptr[1] + (int)((rnd.NextSingle() * 2 - 1) * strength);
                                bn = ptr[2] + (int)((rnd.NextSingle() * 2 - 1) * strength);
                            }
                            // Overwrite old values
                            ptr[0] = rn.ClampToByte();
                            ptr[1] = gn.ClampToByte();
                            ptr[2] = bn.ClampToByte();
                        }
                        ptr += 3;
                    }
                    reporter?.Report(y + 1, 0, image.Height);
                }
            }
            reporter?.Done();
        }
    }
}
