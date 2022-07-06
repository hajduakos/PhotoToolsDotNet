using FilterLib.Reporting;
using Math = System.Math;
using MathF = System.MathF;

namespace FilterLib.Filters.Other
{
    /// <summary>
    /// Waves filter.
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
        public override Image Apply(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            Image wavesBmp;

            int w = image.Width;
            int h = image.Height;

            int waveLengthPx = Wavelength.ToAbsolute(Direction == WaveDirection.Horizontal ? w : h);
            int amplitudePx = Amplitude.ToAbsolute(Direction == WaveDirection.Horizontal ? h : w);

            if (waveLengthPx == 0) throw new System.ArgumentException("Wavelength cannot be zero.");

            if (Direction == WaveDirection.Horizontal) wavesBmp = new Image(w, h + 2 * amplitudePx);
            else wavesBmp = new Image(w + 2 * amplitudePx, h);
            // Lock bits
            unsafe
            {
                fixed (byte* bmdstart = image, newstart = wavesBmp)
                {
                    int width_3 = image.Width * 3; // Width of a row
                    int widthNew_3 = wavesBmp.Width * 3;

                    int x, y, offset, offset_3, idx1, idx2, amplitudePx_3 = amplitudePx * 3;
                    float freq = 2 * MathF.PI / waveLengthPx;

                    if (Direction == WaveDirection.Horizontal) // Horizontal waves
                    {
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            // Calculate offset
                            offset = (int)MathF.Round(MathF.Sin(freq * x / 3) * amplitudePx);
                            if (offset > 0) offset = Math.Min(amplitudePx, offset);
                            else offset = -Math.Min(amplitudePx, -offset);

                            // Iterate through rows and move pixels
                            for (y = 0; y < h; ++y)
                            {
                                idx1 = y * width_3 + x;
                                idx2 = (y + amplitudePx - offset) * widthNew_3 + x;
                                newstart[idx2] = bmdstart[idx1];
                                newstart[idx2 + 1] = bmdstart[idx1 + 1];
                                newstart[idx2 + 2] = bmdstart[idx1 + 2];
                            }

                            reporter?.Report(x, 0, width_3 - 3);
                        }
                    }
                    else // Vertical waves
                    {
                        // Iterate through rows
                        for (y = 0; y < h; ++y)
                        {
                            // Calculate offset
                            offset = (int)MathF.Round(MathF.Sin(freq * y) * amplitudePx);
                            if (offset > 0) offset = Math.Min(amplitudePx, offset);
                            else offset = -Math.Min(amplitudePx, -offset);
                            offset_3 = offset * 3;

                            // Iterate through columns and move pixels
                            for (x = 0; x < width_3; ++x) newstart[y * widthNew_3 + x + amplitudePx_3 - offset_3] = bmdstart[y * width_3 + x];

                            reporter?.Report(y, 0, h - 1);
                        }
                    }
                }
            }
            reporter?.Done();
            return wavesBmp;
        }
    }
}
