using FilterLib.Reporting;
using FilterLib.Util;
using Math = System.Math;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Crop the image to a given rectangular area.
    /// </summary>
    [Filter]
    public sealed class CropFilter : FilterBase
    {
        /// <summary>
        /// Crop area left.
        /// </summary>
        [FilterParam]
        public Size X { get; set; }

        /// <summary>
        /// Crop area top.
        /// </summary>
        [FilterParam]
        public Size Y { get; set; }

        /// <summary>
        /// Crop area width.
        /// </summary>
        [FilterParam]
        public Size Width { get; set; }

        /// <summary>
        /// Crop area height.
        /// </summary>
        [FilterParam]
        public Size Height { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CropFilter() : this(
            Size.Absolute(0),
            Size.Absolute(0),
            Size.Relative(1),
            Size.Relative(1))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Crop area left</param>
        /// <param name="y">Crop area top</param>
        /// <param name="width">Crop area width</param>
        /// <param name="height">Crop area height</param>
        public CropFilter(Size x, Size y, Size width, Size height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <inheritdoc/>
        public override unsafe Image Apply(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            int x0 = X.ToAbsolute(image.Width);
            int y0 = Y.ToAbsolute(image.Height);
            int w0 = Width.ToAbsolute(image.Width);
            int h0 = Height.ToAbsolute(image.Height);
            if (x0 < 0) throw new System.ArgumentException($"X is negative: {x0}");
            if (y0 < 0) throw new System.ArgumentException($"Y is negative: {y0}");
            if (w0 <= 0) throw new System.ArgumentException($"Width is not positive: {w0}");
            if (h0 <= 0) throw new System.ArgumentException($"Height is not positive: {h0}");
            w0 = Math.Min(w0, image.Width);
            h0 = Math.Min(h0, image.Height);

            Image cropped = new(w0, h0);
            int newWidth_3 = cropped.Width * 3;
            int oldWidth_3 = image.Width * 3;
            int x0_3 = x0 * 3;

            fixed (byte* newStart = cropped, oldStart = image)
            {
                for (int y = 0; y < h0; ++y)
                {
                    byte* newRow = newStart + y * newWidth_3;
                    byte* oldRow = oldStart + (y + y0) * oldWidth_3;
                    for (int x = 0; x < newWidth_3; x += 3)
                    {
                        newRow[x] = oldRow[x0_3 + x];
                        newRow[x + 1] = oldRow[x0_3 + x + 1];
                        newRow[x + 2] = oldRow[x0_3 + x + 2];
                    }
                    reporter?.Report(y + 1, 0, h0);
                }
            }
            reporter?.Done();
            return cropped;
        }
    }
}
