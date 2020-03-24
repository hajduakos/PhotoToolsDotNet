using System.Drawing;
using System.Drawing.Imaging;
using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Base class for ordered dithering filters.
    /// </summary>
    public abstract class OrderedDitherFilterBase : FilterInPlaceBase
    {
        private int levels;

        /// <summary>
        /// Number of levels [2:256].
        /// </summary>
        [FilterParam]
        [FilterParamMin(2)]
        [FilterParamMax(256)]
        public int Levels
        {
            get { return levels; }
            set { levels = value.Clamp(2, 256); }
        }

        /// <summary>
        /// Dither matrix.
        /// </summary>
        protected IOrderedDitherMatrix Matrix { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2;256]</param>
        /// <param name="matrix">Dither matrix</param>
        protected OrderedDitherFilterBase(int levels, IOrderedDitherMatrix matrix)
        {
            this.Levels = levels;
            this.Matrix = matrix;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new DisposableBitmapData(image, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3;
                int h = image.Height;
                int x, y, xDiv3;
                int mw = Matrix.Width, mh = Matrix.Height; // Width and height of matrix
                double intervalSize = 255f / (levels - 1); // Size of an interval
                double roundedColor; // Color rounded to the nearest color level
                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; y++)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; ++x)
                        {
                            xDiv3 = x / 3;
                            // Get rounded color
                            roundedColor = System.Math.Floor(row[x] / intervalSize) * intervalSize;
                            // Calculate new value using dither matrix
                            row[x] = (byte)((roundedColor + Matrix[xDiv3 % mw, y % mh] * intervalSize > row[x]) ? roundedColor : (roundedColor + intervalSize));
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
