using FilterLib.Reporting;
using FilterLib.Util;
using System;

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
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            unsafe
            {
                fixed (byte* start = image)
                {
                    Random rnd = new(Seed);
                    int width_3 = image.Width * 3;
                    int borderWidth = Width.ToAbsolute(Math.Max(image.Width, image.Height));
                    // Starting from the edge of the image, and going inwards, the probability of
                    // coloring a pixel fades off in with a sine transition for smooth results
                    float[] map = new float[borderWidth + 1];
                    for (int x = 0; x <= borderWidth; ++x)
                        map[x] = MathF.Sin(x / (float)borderWidth * MathF.PI - MathF.PI / 2) / 2 + .5f;
                    // Iterate through rows
                    for (int y = 0; y < image.Height; ++y)
                    {
                        // Get rows
                        byte* row = start + (y * width_3);
                        // Iterate through columns
                        for (int x = 0; x < width_3; x += 3)
                        {
                            int distanceFromNearestEdge = Math.Min(Math.Min(x / 3, y), Math.Min(image.Width - x / 3 - 1, image.Height - y - 1));
                            if (distanceFromNearestEdge > borderWidth) continue;
                            if (rnd.NextSingle() > map[distanceFromNearestEdge])
                            {
                                row[x + 2] = Color.R;
                                row[x + 1] = Color.G;
                                row[x] = Color.B;
                            }
                        }
                        reporter?.Report(y, 0, image.Height - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
