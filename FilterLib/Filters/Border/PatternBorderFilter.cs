using FilterLib.Reporting;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Pattern border filter.
    /// </summary>
    [Filter]
    public sealed class PatternBorderFilter : IFilter
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
        /// Border pattern.
        /// </summary>
        [FilterParam]
        public Bitmap Pattern { get; set; }

        /// <summary>
        /// Border position.
        /// </summary>
        [FilterParam]
        public BorderPosition Position { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PatternBorderFilter() :
            this(Util.Size.Absolute(0), Util.Size.Absolute(0), new Bitmap(1, 1), BorderPosition.Inside)
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Border width</param>
        /// <param name="radius">Border radius</param>
        /// <param name="pattern">Border pattern</param>
        /// <param name="position">Border position</param>
        public PatternBorderFilter(Util.Size width, Util.Size radius, Bitmap pattern, BorderPosition position)
        {
            this.Width = width;
            this.Radius = radius;
            this.Pattern = pattern;
            this.Position = position;
        }

        public Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            int w = image.Width;
            int h = image.Height;
            int borderW = Width.ToAbsolute(Math.Max(w, h));
            int borderRad = Radius.ToAbsolute(Math.Max(w, h));
            if (Position == BorderPosition.Outside) { w += 2 * borderW; h += 2 * borderW; }
            else if (Position == BorderPosition.Center) { w += borderW; h += borderW; }

            Bitmap imageWithBorder = new Bitmap(w, h);
            using (Graphics gfx = Graphics.FromImage(imageWithBorder))
            using (Brush brush = new TextureBrush(Pattern))
            {
                gfx.FillRectangle(brush, 0, 0, w, h);
                gfx.SmoothingMode = SmoothingMode.HighQuality;
                gfx.SetClip(new Rectangle(borderW, borderW + borderRad, w - 2 * borderW, h - 2 * borderW - 2 * borderRad));
                gfx.SetClip(new Rectangle(borderW + borderRad, borderW, w - 2 * borderW - 2 * borderRad, h - 2 * borderW), CombineMode.Union);
                GraphicsPath clip = new GraphicsPath
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
