using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterLib.Filters.Other
{
    /// <summary>
    /// Waves filter.
    /// </summary>
    [Filter]
    public sealed class WavesFilter : IFilter
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
            this.Wavelength = wavelength;
            this.Amplitude = amplitude;
            this.Direction = direction;
        }

        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        public Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            Bitmap wavesBmp;

            int w = image.Width;
            int h = image.Height;

            int waveLengthPx = Wavelength.ToAbsolute(Direction == WaveDirection.Horizontal ? w : h);
            int amplitudePx = Amplitude.ToAbsolute(Direction == WaveDirection.Horizontal ? h : w);

            if (waveLengthPx == 0) throw new ArgumentException("Wavelength cannot be zero.");

            if (Direction == WaveDirection.Horizontal) wavesBmp = new Bitmap(w, h + 2 * amplitudePx);
            else wavesBmp = new Bitmap(w + 2 * amplitudePx, h);
            using (Graphics gfx = Graphics.FromImage(wavesBmp))
            {
                gfx.Clear(System.Drawing.Color.Black);
            }
            // Lock bits
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdNew = new(wavesBmp, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3; // Width of a row
                int stride = bmd.Stride;
                int strideNew = bmdNew.Stride;


                int x, y, offset, offsetMul3, idx1, idx2, amplitudePxMul3 = amplitudePx * 3;
                float freq = 2 * MathF.PI / waveLengthPx;
                unsafe
                {
                    byte* bmdstart = (byte*)bmd.Scan0;
                    byte* newstart = (byte*)bmdNew.Scan0;

                    if (Direction == WaveDirection.Horizontal) // Horizontal waves
                    {
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                        {
                            // Calculate offset
                            offset = (int)MathF.Round(MathF.Sin(freq * x / 3) * amplitudePx);
                            if (offset > 0) offset = Math.Min(amplitudePx, offset);
                            else offset = -Math.Min(amplitudePx, -offset);

                            // Iterate through rows and move pixels
                            for (y = 0; y < h; ++y)
                            {
                                idx1 = y * stride + x;
                                idx2 = (y + amplitudePx - offset) * strideNew + x;
                                newstart[idx2] = bmdstart[idx1];
                                newstart[idx2 + 1] = bmdstart[idx1 + 1];
                                newstart[idx2 + 2] = bmdstart[idx1 + 2];
                            }

                            if ((x & 63) == 0) reporter?.Report(x, 0, wMul3 - 3);
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
                            offsetMul3 = offset * 3;

                            // Iterate through columns and move pixels
                            for (x = 0; x < wMul3; ++x) newstart[y * strideNew + x + amplitudePxMul3 - offsetMul3] = bmdstart[y * stride + x];

                            if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                        }
                    }
                }
            }
            reporter?.Done();
            return wavesBmp;
        }
    }
}
