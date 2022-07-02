using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Crop filter.
    /// </summary>
    [Filter]
    public sealed class CropFilter : FilterBase
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
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <inheritdoc/>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            int x0 = X.ToAbsolute(image.Width);
            int y0 = Y.ToAbsolute(image.Height);
            int w0 = Width.ToAbsolute(image.Width);
            int h0 = Height.ToAbsolute(image.Height);
            if (x0 < 0) throw new System.ArgumentException($"Ivalid X: {x0}");
            if (y0 < 0) throw new System.ArgumentException($"Ivalid Y: {y0}");
            if (w0 <= 0) throw new System.ArgumentException($"Ivalid Width: {w0}");
            if (h0 <= 0) throw new System.ArgumentException($"Ivalid Height: {h0}");
            w0 = Math.Min(w0, image.Width);
            h0 = Math.Min(h0, image.Height);

            Bitmap cropped = new(w0, h0);
            int x, y;
            int width_3 = cropped.Width * 3;
            int x0_3 = x0 * 3;
            using (DisposableBitmapData bmd = new(cropped, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdOrig = new(image, PixelFormat.Format24bppRgb))
            {
                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h0; ++y)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        byte* rowOrig = (byte*)bmdOrig.Scan0 + ((y + y0) * bmdOrig.Stride);
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            row[x] = rowOrig[x0_3 + x];
                            row[x + 1] = rowOrig[x0_3 + x + 1];
                            row[x + 2] = rowOrig[x0_3 + x + 2];
                        }
                        reporter?.Report(y, 0, h0 - 1);
                    }
                }
            }
            reporter?.Done();
            return cropped;
        }
    }
}
