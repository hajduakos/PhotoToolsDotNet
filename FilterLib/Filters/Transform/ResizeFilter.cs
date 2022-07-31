using FilterLib.Reporting;
using Math = System.Math;
using FilterLib.Util;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Resize the image (both up- and downscale).
    /// </summary>
    [Filter]
    public sealed class ResizeFilter : FilterBase
    {
        /// <summary>
        /// New width.
        /// </summary>
        [FilterParam]
        public Size Width { get; set; }

        /// <summary>
        /// New height.
        /// </summary>
        [FilterParam]
        public Size Height { get; set; }

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
        public ResizeFilter(Size width, Size height, InterpolationMode interpolation = InterpolationMode.NearestNeighbor)
        {
            Width = width;
            Height = height;
            Interpolation = interpolation;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ResizeFilter() : this(Size.Relative(1f), Size.Relative(1f)) { }

        /// <inheritdoc/>
        public override unsafe Image Apply(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            int newWidth = Width.ToAbsolute(image.Width);
            int newHeight = Height.ToAbsolute(image.Height);

            if (newWidth <= 0) throw new System.ArgumentException($"New width must be positive: {newWidth}.");
            if (newHeight <= 0) throw new System.ArgumentException($"New height must be positive: {newHeight}.");

            Image resized = new(newWidth, newHeight);

            int newWidth_3 = resized.Width * 3;
            int x0, y0, x1, y1;
            // We want to map [0; w-1] to [0; w'-1], hence the (-1) adjustment
            float wScale = resized.Width > 1 ? (image.Width - 1) / (float)(resized.Width - 1) : 0;
            float hScale = resized.Height > 1 ? (image.Height - 1) / (float)(resized.Height - 1) : 0;

            fixed (byte* newStart = resized, oldStart = image)
            {
                for (int y = 0; y < resized.Height; ++y)
                {
                    byte* newRow = newStart + y * newWidth_3;
                    for (int x = 0; x < newWidth_3; x += 3)
                    {
                        switch (Interpolation)
                        {
                            case InterpolationMode.NearestNeighbor:
                                y0 = (int)Math.Round(y * hScale);
                                x0 = (int)Math.Round(x / 3 * wScale) * 3;
                                byte* oldRow = oldStart + (y0 * image.Width * 3);
                                for (int i = 0; i < 3; i++) newRow[x + i] = oldRow[x0 + i];
                                break;
                            case InterpolationMode.Bilinear:
                                float yf = y * hScale;
                                y0 = (int)Math.Floor(yf);
                                y1 = (int)Math.Ceiling(yf);
                                float yRatio1 = yf - y0;
                                float yRatio0 = 1 - yRatio1;
                                float xf = x / 3 * wScale;
                                x0 = (int)Math.Floor(xf) * 3;
                                x1 = (int)Math.Ceiling(xf) * 3;
                                float xRatio1 = xf - x0 / 3;
                                float xRatio0 = 1 - xRatio1;
                                byte* oldRow0 = oldStart + (y0 * image.Width * 3);
                                byte* oldRow1 = oldStart + (y1 * image.Width * 3);
                                for (int i = 0; i < 3; i++)
                                    newRow[x + i] = (byte)(
                                        yRatio0 * xRatio0 * oldRow0[x0 + i] +
                                        yRatio1 * xRatio0 * oldRow1[x0 + i] +
                                        yRatio0 * xRatio1 * oldRow0[x1 + i] +
                                        yRatio1 * xRatio1 * oldRow1[x1 + i]);
                                break;
                            default:
                                throw new System.ArgumentException($"Unknown interpolation mode: {Interpolation}.");
                        }
                    }
                    reporter?.Report(y + 1, 0, resized.Height);
                }
            }

            reporter?.Done();
            return resized;
        }
    }
}
