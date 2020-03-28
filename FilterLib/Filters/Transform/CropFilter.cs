using FilterLib.Reporting;
using System;
using System.Drawing;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Crop filter.
    /// </summary>
    [Filter]
    public sealed class CropFilter : IFilter
    {
        /// <summary>
        /// Crop area left.
        /// </summary>
        [FilterParam]
        public Util.Size X { get; set; }

        /// <summary>
        /// Crop area top.
        /// </summary>
        [FilterParam]
        public Util.Size Y { get; set; }

        /// <summary>
        /// Crop area width.
        /// </summary>
        [FilterParam]
        public Util.Size Width { get; set; }

        /// <summary>
        /// Crop area height.
        /// </summary>
        [FilterParam]
        public Util.Size Height { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CropFilter() : this(
            Util.Size.Absolute(0),
            Util.Size.Absolute(0),
            Util.Size.Relative(1),
            Util.Size.Relative(1))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Crop area left</param>
        /// <param name="y">Crop area top</param>
        /// <param name="width">Crop area width</param>
        /// <param name="height">Crop area height</param>
        public CropFilter(Util.Size x, Util.Size y, Util.Size width, Util.Size height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        public Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            int x0 = X.ToAbsolute(image.Width);
            int y0 = Y.ToAbsolute(image.Height);
            int w0 = Width.ToAbsolute(image.Width);
            int h0 = Height.ToAbsolute(image.Height);
            if (x0 < 0) throw new ArgumentException($"Ivalid X: {x0}");
            if (y0 < 0) throw new ArgumentException($"Ivalid Y: {y0}");
            if (w0 <= 0) throw new ArgumentException($"Ivalid Width: {w0}");
            if (h0 <= 0) throw new ArgumentException($"Ivalid Height: {h0}");

            Bitmap cropped = new Bitmap(w0, h0);
            using (Graphics gfx = Graphics.FromImage(cropped))
            {
                gfx.DrawImage(image, -x0, -y0, image.Width, image.Height);
            }
            return cropped;
        }
    }
}
