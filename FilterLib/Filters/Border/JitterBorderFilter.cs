using FilterLib.Reporting;
using FilterLib.Util;
using System;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Add a jitter border, where the probability of coloring
    /// a pixel gradually goes from 1 to 0 towards the center.
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
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();

            fixed (byte* start = image)
            {
                Random rnd = new(Seed);
                int borderWidth = Width.ToAbsolute(Math.Max(image.Width, image.Height));
                // Starting from the edge of the image, and going inwards, the probability of
                // coloring a pixel fades off in with a sine transition for smooth results
                float[] map = new float[borderWidth + 1];
                for (int x = 0; x <= borderWidth; ++x)
                    map[x] = MathF.Sin(x / (float)borderWidth * MathF.PI - MathF.PI / 2) / 2 + .5f;

                byte* ptr = start;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        int distanceFromNearestEdge = Math.Min(Math.Min(x, y), Math.Min(image.Width - x - 1, image.Height - y - 1));
                        if (distanceFromNearestEdge <= borderWidth && rnd.NextSingle() > map[distanceFromNearestEdge])
                        {
                            ptr[0] = Color.R;
                            ptr[1] = Color.G;
                            ptr[2] = Color.B;
                        }
                        ptr += 3;
                    }
                    reporter?.Report(y, 0, image.Height - 1);
                }
            }
            reporter?.Done();
        }
    }
}