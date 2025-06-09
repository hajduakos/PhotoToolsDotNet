using FilterLib.Reporting;
using FilterLib.Util;
using Math = System.Math;
using MathF = System.MathF;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Other
{
    /// <summary>
    /// Distort the image as if waves were going through it.
    /// </summary>
    [Filter]
    public sealed class WavesFilter : FilterBase
    {
        /// <summary>
        /// Wavelength.
        /// </summary>
        [FilterParam]
        public Util.Size Wavelength { get; set; }

        /// <summary>
        /// Amplitude.
        /// </summary>
        [FilterParam]
        public Util.Size Amplitude { get; set; }

        /// <summary>
        /// Direction.
        /// </summary>
        [FilterParam]
        public Direction Direction { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public WavesFilter() : this(Util.Size.Relative(1), Util.Size.Absolute(0)) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="wavelength">Wavelength</param>
        /// <param name="amplitude">Amplitude</param>
        /// <param name="direction">Direction</param>
        public WavesFilter(Util.Size wavelength, Util.Size amplitude, Direction direction = Direction.Horizontal)
        {
            Wavelength = wavelength;
            Amplitude = amplitude;
            Direction = direction;
        }

        /// <inheritdoc/>
        public override unsafe Image Apply(Image image, IReporter reporter = null)
        {
            reporter?.Start();

            int waveLengthPx = Wavelength.ToAbsolute(Direction == Direction.Horizontal ? image.Width : image.Height);
            int amplitudePx = Amplitude.ToAbsolute(Direction == Direction.Horizontal ? image.Height : image.Width);
            if (waveLengthPx == 0) throw new System.ArgumentException("Wavelength cannot be zero.");

            Image result = Direction switch
            {
                Direction.Horizontal => new(image.Width, image.Height + 2 * amplitudePx),
                Direction.Vertical => new(image.Width + 2 * amplitudePx, image.Height),
                _ => throw new System.ArgumentException($"Unknown wave direction: {Direction}.")
            };

            int oldWidth_3 = image.Width * 3;
            int newWidth_3 = result.Width * 3;
            int amplitudePx_3 = amplitudePx * 3;
            float freq = 2 * MathF.PI / waveLengthPx;

            fixed (byte* oldStart = image, newStart = result)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                if (Direction == Direction.Horizontal)
                {
                    // Iterate through columns
                    for (int x = 0; x < oldWidth_3; ++x)
                    {
                        // Calculate offset
                        int x_div_3 = x / 3;
                        int offset = (int)MathF.Round(MathF.Sin(freq * x_div_3) * amplitudePx);
                        if (offset > 0) offset = Math.Min(amplitudePx, offset);
                        else offset = -Math.Min(amplitudePx, -offset);

                        // Iterate through rows and move pixels
                        Parallel.For(0, image.Height, y =>
                        {
                            newStart0[(y + amplitudePx - offset) * newWidth_3 + x] = oldStart0[y * oldWidth_3 + x];
                        });
                        reporter?.Report(x + 3, 0, oldWidth_3);
                    }
                }
                else if (Direction == Direction.Vertical)
                {
                    // Iterate through rows
                    for (int y = 0; y < image.Height; ++y)
                    {
                        // Calculate offset
                        int offset = (int)MathF.Round(MathF.Sin(freq * y) * amplitudePx);
                        if (offset > 0) offset = Math.Min(amplitudePx, offset);
                        else offset = -Math.Min(amplitudePx, -offset);
                        int offset_3 = offset * 3;

                        // Iterate through columns and move pixels
                        Parallel.For(0, oldWidth_3, x =>
                        {
                            newStart0[y * newWidth_3 + x + amplitudePx_3 - offset_3] = oldStart0[y * oldWidth_3 + x];
                        });
                        reporter?.Report(y + 1, 0, image.Height);
                    }
                }
                else throw new System.ArgumentException($"Unknown wave direction: {Direction}.");
            }
            reporter?.Done();
            return result;
        }
    }
}
