using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Fade border filter.
    /// </summary>
    [Filter]
    public sealed class FadeBorderFilter : FilterInPlaceBase
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
        /// Constructor.
        /// </summary>
        public FadeBorderFilter() : this(Util.Size.Absolute(0), new RGB(0, 0, 0)) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Border width</param>
        /// <param name="color">Border color</param>
        public FadeBorderFilter(Util.Size width, RGB color)
        {
            Width = width;
            Color = color;
        }

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            int borderWidth = Width.ToAbsolute(Math.Max(image.Width, image.Height));
            int width_3 = image.Width * 3;
            using DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb);
            unsafe
            {
                // Draw rectangles with decreasing alpha value
                for (int k = 0; k < borderWidth; ++k)
                {
                    float alpha = 1 - k / (float)borderWidth;
                    if (k < image.Height)
                    {
                        for (int x = 0; x < width_3; x += 3)
                        {
                            byte* px = (byte*)bmd.Scan0 + k * bmd.Stride + x;
                            px[0] = (byte)(px[0] * (1 - alpha) + Color.B * alpha);
                            px[1] = (byte)(px[1] * (1 - alpha) + Color.G * alpha);
                            px[2] = (byte)(px[2] * (1 - alpha) + Color.R * alpha);
                            px = (byte*)bmd.Scan0 + (image.Height - 1 - k) * bmd.Stride + x;
                            px[0] = (byte)(px[0] * (1 - alpha) + Color.B * alpha);
                            px[1] = (byte)(px[1] * (1 - alpha) + Color.G * alpha);
                            px[2] = (byte)(px[2] * (1 - alpha) + Color.R * alpha);
                        }
                    }
                    if (k < image.Width)
                    {
                        for (int y = 0; y < image.Height; ++y)
                        {
                            byte* px = (byte*)bmd.Scan0 + y * bmd.Stride + k * 3;
                            px[0] = (byte)(px[0] * (1 - alpha) + Color.B * alpha);
                            px[1] = (byte)(px[1] * (1 - alpha) + Color.G * alpha);
                            px[2] = (byte)(px[2] * (1 - alpha) + Color.R * alpha);
                            px = (byte*)bmd.Scan0 + y * bmd.Stride + width_3 - (k + 1) * 3;
                            px[0] = (byte)(px[0] * (1 - alpha) + Color.B * alpha);
                            px[1] = (byte)(px[1] * (1 - alpha) + Color.G * alpha);
                            px[2] = (byte)(px[2] * (1 - alpha) + Color.R * alpha);
                        }
                    }
                    reporter?.Report(k, 0, borderWidth - 1);
                }
            }
            reporter?.Done();
        }
    }
}
