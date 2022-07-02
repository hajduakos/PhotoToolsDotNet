using FilterLib.Reporting;
using FilterLib.Util;
using System;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Jitter border filter.
    /// </summary>
    [Filter]
    public sealed class JitterBorderFilter : FilterInPlaceBase
    {
        /// <summary>
        /// Border width.
        /// </summary>
        [FilterParam]
        public Util.Size Width { get; set; }

        /// <summary>
        /// Border color.
        /// </summary>
        [FilterParam]
        public RGB Color { get; set; }

        /// <summary>
        /// Random generator seed.
        /// </summary>
        [FilterParam]
        public int Seed { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public JitterBorderFilter() : this(Util.Size.Absolute(0), new RGB(0, 0, 0), 0) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Border width</param>
        /// <param name="color">Border color</param>
        /// <param name="seed">Random generator seed</param>
        public JitterBorderFilter(Util.Size width, RGB color, int seed = 0)
        {
            Width = width;
            Color = color;
            Seed = seed;
        }

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Lock bits
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                // Random number generator
                Random rnd = new(Seed);
                int w = image.Width, h = image.Height;
                int stride = bmd.Stride;
                int width_3 = image.Width * 3; // Width of a row
                int x, y, distanceFromNearestEdge;
                int borderWidth = Width.ToAbsolute(Math.Max(w, h));
                double[] map = new double[borderWidth + 1];
                for (x = 0; x <= borderWidth; ++x) map[x] = MathF.Sin(x / (float)borderWidth * MathF.PI - MathF.PI / 2) / 2 + 0.5;
                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmd.Scan0 + (y * stride);
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            distanceFromNearestEdge = Math.Min(Math.Min(x / 3, y), Math.Min(w - x / 3 - 1, h - y - 1));
                            if (distanceFromNearestEdge > borderWidth) continue;
                            if (rnd.NextDouble() > map[distanceFromNearestEdge])
                            {
                                row[x + 2] = (byte)Color.R;
                                row[x + 1] = (byte)Color.G;
                                row[x] = (byte)Color.B;
                            }
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
