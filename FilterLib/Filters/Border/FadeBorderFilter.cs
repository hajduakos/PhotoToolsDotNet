using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;

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
            int w = image.Width, h = image.Height;
            int borderWidth = Width.ToAbsolute(Math.Max(w, h));
            using (Graphics gfx = Graphics.FromImage(image))
            {
                // Draw rectangles with decreasing alpha value
                for (int k = 0; k < borderWidth; ++k)
                {
                    using Pen pen = new(System.Drawing.Color.FromArgb((int)(255 - k / (float)(borderWidth - 1) * 255), Color.R, Color.G, Color.B));
                    gfx.DrawLine(pen, 0, k, w, k);
                    gfx.DrawLine(pen, k, 0, k, h);
                    gfx.DrawLine(pen, 0, h - 1 - k, w, h - 1 - k);
                    gfx.DrawLine(pen, w - 1 - k, 0, w - 1 - k, h);
                    reporter?.Report(k, 0, borderWidth - 1);
                }
            }
            reporter?.Done();
        }
    }
}
