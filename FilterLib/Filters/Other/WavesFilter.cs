using FilterLib.Reporting;
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
        /// Wave directions.
        /// </summary>
        public enum WaveDirection { Horizontal, Vertical }

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
        public WaveDirection Direction { get; set; }

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
        public WavesFilter(Util.Size wavelength, Util.Size amplitude, WaveDirection direction = WaveDirection.Horizontal)
        {
            Wavelength = wavelength;
            Amplitude = amplitude;
            Direction = direction;
        }

        /// <inheritdoc/>
        public override unsafe Image Apply(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            Image result;

            int waveLengthPx = Wavelength.ToAbsolute(Direction == WaveDirection.Horizontal ? image.Width : image.Height);
            int amplitudePx = Amplitude.ToAbsolute(Direction == WaveDirection.Horizontal ? image.Height : image.Width);

            if (waveLengthPx == 0) throw new System.ArgumentException("Wavelength cannot be zero.");

            if (Direction == WaveDirection.Horizontal) result = new(image.Width, image.Height + 2 * amplitudePx);
            else result = new(image.Width + 2 * amplitudePx, image.Height);

            int oldWidth_3 = image.Width * 3;
            int newWidth_3 = result.Width * 3;
            int amplitudePx_3 = amplitudePx * 3;
            float freq = 2 * MathF.PI / waveLengthPx;

            fixed (byte* oldStart = image, newStart = result)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                if (Direction == WaveDirection.Horizontal)
                {
                    // Iterate through columns
                    for (int x = 0; x < oldWidth_3; x += 3)
                    {
                        // Calculate offset
                        int offset = (int)MathF.Round(MathF.Sin(freq * x / 3) * amplitudePx);
                        if (offset > 0) offset = Math.Min(amplitudePx, offset);
                        else offset = -Math.Min(amplitudePx, -offset);

                        // Iterate through rows and move pixels
                        Parallel.For(0, image.Height, y =>
                        {
                            int idx1 = y * oldWidth_3 + x;
                            int idx2 = (y + amplitudePx - offset) * newWidth_3 + x;
                            newStart0[idx2] = oldStart0[idx1];
                            newStart0[idx2 + 1] = oldStart0[idx1 + 1];
                            newStart0[idx2 + 2] = oldStart0[idx1 + 2];
                        });

                        reporter?.Report(x + 3, 0, oldWidth_3);
                    }
                }
                else if (Direction == WaveDirection.Vertical)
                {
                    // Iterate through rows
                    Parallel.For(0, image.Height, y =>
                    {
                        // Calculate offset
                        int offset = (int)MathF.Round(MathF.Sin(freq * y) * amplitudePx);
                        if (offset > 0) offset = Math.Min(amplitudePx, offset);
                        else offset = -Math.Min(amplitudePx, -offset);
                        int offset_3 = offset * 3;

                        // Iterate through columns and move pixels
                        for (int x = 0; x < oldWidth_3; ++x)
                            newStart0[y * newWidth_3 + x + amplitudePx_3 - offset_3] = oldStart0[y * oldWidth_3 + x];

                        if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                    });
                }
                else throw new System.ArgumentException($"Unknown wave direction: {Direction}.");
            }
            reporter?.Done();
            return result;
        }
    }
}
