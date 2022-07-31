using FilterLib.Reporting;
using FilterLib.Util;
using System;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Apply a border that gradually fades away going towards the center.
    /// </summary>
    [Filter]
    public sealed class FadeBorderFilter : FilterInPlaceBase
    {
        /// <summary>
        /// Border width.
        /// </summary>
        [FilterParam]
        public Size Width { get; set; }

        /// <summary>
        /// Border color.
        /// </summary>
        [FilterParam]
        public RGB Color { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FadeBorderFilter() : this(Size.Absolute(0), new RGB(0, 0, 0)) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Border width</param>
        /// <param name="color">Border color</param>
        public FadeBorderFilter(Size width, RGB color)
        {
            Width = width;
            Color = color;
        }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            int borderWidth = Width.ToAbsolute(Math.Max(image.Width, image.Height));
            int width_3 = image.Width * 3;

            fixed (byte* start = image)
            {
                // Draw rectangles with decreasing alpha value
                for (int k = 0; k < borderWidth; ++k)
                {
                    float alpha = 1 - k / (float)borderWidth;
                    if (k < image.Height)
                    {
                        for (int x = 0; x < width_3; x += 3)
                        {
                            byte* px = start + k * width_3 + x;
                            px[0] = (byte)(px[0] * (1 - alpha) + Color.R * alpha);
                            px[1] = (byte)(px[1] * (1 - alpha) + Color.G * alpha);
                            px[2] = (byte)(px[2] * (1 - alpha) + Color.B * alpha);
                            px = start + (image.Height - 1 - k) * width_3 + x;
                            px[0] = (byte)(px[0] * (1 - alpha) + Color.R * alpha);
                            px[1] = (byte)(px[1] * (1 - alpha) + Color.G * alpha);
                            px[2] = (byte)(px[2] * (1 - alpha) + Color.B * alpha);
                        }
                    }
                    if (k < image.Width)
                    {
                        for (int y = 0; y < image.Height; ++y)
                        {
                            byte* px = start + y * width_3 + k * 3;
                            px[0] = (byte)(px[0] * (1 - alpha) + Color.R * alpha);
                            px[1] = (byte)(px[1] * (1 - alpha) + Color.G * alpha);
                            px[2] = (byte)(px[2] * (1 - alpha) + Color.B * alpha);
                            px = start + y * width_3 + width_3 - (k + 1) * 3;
                            px[0] = (byte)(px[0] * (1 - alpha) + Color.R * alpha);
                            px[1] = (byte)(px[1] * (1 - alpha) + Color.G * alpha);
                            px[2] = (byte)(px[2] * (1 - alpha) + Color.B * alpha);
                        }
                    }
                    reporter?.Report(k + 1, 0, borderWidth);
                }
            }
            reporter?.Done();
        }
    }
}
