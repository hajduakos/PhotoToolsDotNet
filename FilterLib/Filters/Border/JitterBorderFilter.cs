using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;

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
            this.Width = width;
            this.Color = color;
            this.Seed = seed;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Lock bits
            using (DisposableBitmapData bmd = new DisposableBitmapData(image, PixelFormat.Format24bppRgb))
            {
                // Random number generator
                Random rnd = new Random(Seed);
                int w = image.Width, h = image.Height;
                int stride = bmd.Stride;
                int wMul3 = image.Width * 3; // Width of a row
                int x, y, distanceFromNearestEdge;
                int borderWidth = Width.ToAbsolute(Math.Max(w, h));
                double[] map = new double[borderWidth + 1];
                for (x = 0; x <= borderWidth; ++x) map[x] = Math.Sin(x / (double)borderWidth * Math.PI - Math.PI / 2) / 2 + 0.5;
                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmd.Scan0 + (y * stride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
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
