using FilterLib.Reporting;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Resize filter.
    /// </summary>
    [Filter]
    public sealed class ResizeFilter : FilterBase
    {
        /// <summary>
        /// New width.
        /// </summary>
        [FilterParam]
        public Util.Size Width { get; set; }

        /// <summary>
        /// New height.
        /// </summary>
        [FilterParam]
        public Util.Size Height { get; set; }

        /// <summary>
        /// Interpolation mode.
        /// </summary>
        [FilterParam]
        public InterpolationMode Interpolation { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        /// <param name="interpolation">Interpolation mode</param>
        public ResizeFilter(Util.Size width, Util.Size height, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic)
        {
            this.Width = width;
            this.Height = height;
            this.Interpolation = interpolation;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ResizeFilter() : this(Util.Size.Relative(1f), Util.Size.Relative(1f)) { }

        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            int newWidth = Width.ToAbsolute(image.Width);
            int newHeight = Height.ToAbsolute(image.Height);

            if (newWidth <= 0) throw new ArgumentException($"Invalid new width: {newWidth}.");
            if (newHeight <= 0) throw new ArgumentException($"Invalid new height: {newHeight}.");

            Bitmap resized = new(newWidth, newHeight);
            using (Graphics gfx = Graphics.FromImage(resized))
            {
                gfx.InterpolationMode = Interpolation;
                gfx.DrawImage(image, 0, 0, resized.Width, resized.Height);
            }
            reporter?.Done();
            return resized;
        }
    }
}
