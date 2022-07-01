using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Simple border filter.
    /// </summary>
    [Filter]
    public sealed class SimpleBorderFilter : FilterBase
    {
        /// <summary>
        /// Border width.
        /// </summary>
        [FilterParam]
        public Util.Size Width { get; set; }

        /// <summary>
        /// Border radius.
        /// </summary>
        [FilterParam]
        public Util.Size Radius { get; set; }

        /// <summary>
        /// Border color.
        /// </summary>
        [FilterParam]
        public RGB Color { get; set; }

        /// <summary>
        /// Border position.
        /// </summary>
        [FilterParam]
        public BorderPosition Position { get; set; }

        public SimpleBorderFilter() :
            this(Util.Size.Absolute(0), Util.Size.Absolute(0), new RGB(0, 0, 0), BorderPosition.Inside) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Border width</param>
        /// <param name="radius">Border radius</param>
        /// <param name="color">Border color</param>
        /// <param name="position">Border position</param>
        public SimpleBorderFilter(Util.Size width, Util.Size radius, RGB color, BorderPosition position)
        {
            Width = width;
            Radius = radius;
            Color = color;
            Position = position;
        }

        /// <inheritdoc/>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            int w = image.Width;
            int h = image.Height;
            int borderW = Width.ToAbsolute(Math.Max(w, h));
            if (borderW == 0) return (Bitmap)image.Clone();
            int borderRad = Radius.ToAbsolute(Math.Max(w, h));
            if (Position == BorderPosition.Outside) { w += 2 * borderW; h += 2 * borderW; }
            else if (Position == BorderPosition.Center) { w += borderW; h += borderW; }

            Bitmap imageWithBorder = new(w, h);
            using (Graphics gfx = Graphics.FromImage(imageWithBorder))
            {
                gfx.SmoothingMode = SmoothingMode.HighQuality;
                gfx.Clear(System.Drawing.Color.FromArgb(Color.R, Color.G, Color.B));
                gfx.SetClip(new Rectangle(borderW, borderW + borderRad, w - 2 * borderW, h - 2 * borderW - 2 * borderRad));
                gfx.SetClip(new Rectangle(borderW + borderRad, borderW, w - 2 * borderW - 2 * borderRad, h - 2 * borderW), CombineMode.Union);
                GraphicsPath clip = new()
                {
                    FillMode = FillMode.Winding
                };
                clip.AddEllipse(borderW, borderW, borderRad * 2, borderRad * 2);
                clip.AddEllipse(w - borderW - 2 * borderRad, borderW, 2 * borderRad, 2 * borderRad);
                clip.AddEllipse(borderW, h - borderW - 2 * borderRad, 2 * borderRad, 2 * borderRad);
                clip.AddEllipse(w - borderW - 2 * borderRad, h - borderW - 2 * borderRad, 2 * borderRad, 2 * borderRad);

                gfx.SetClip(clip, CombineMode.Union);

                gfx.DrawImage(image, (w - image.Width) / 2, (h - image.Height) / 2, image.Width, image.Height);
            }
            reporter?.Done();
            return imageWithBorder;
        }
    }
}
