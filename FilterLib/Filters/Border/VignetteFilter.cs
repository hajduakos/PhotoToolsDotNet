using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Vignette filter.
    /// </summary>
    [Filter]
    public sealed class VignetteFilter : FilterInPlaceBase
    {

        /// <summary>
        /// Radius of the vignette, outside is black.
        /// </summary>
        [FilterParam]
        public Util.Size Radius { get; set; }

        /// <summary>
        /// Radius of the clear zone.
        /// </summary>
        [FilterParam]
        public Util.Size ClearRadius { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public VignetteFilter() : this(Util.Size.Relative(3), Util.Size.Relative(2)) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="radius">Radius of the vignette</param>
        /// <param name="clearRadius">Radius of the clear zone</param>
        public VignetteFilter(Util.Size radius, Util.Size clearRadius)
        {
            this.Radius = radius;
            this.ClearRadius = clearRadius;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3;
                int w = image.Width, h = image.Height;
                int x, y;
                float xShifted, yShifted, ellipseRadius;
                float a1 = Radius.ToAbsolute(w / 2);
                float a0 = ClearRadius.ToAbsolute(w / 2);
                if (a1 < a0) throw new ArgumentException("Radius must be larger than clear zone radius.");
                float halfWidth = w / 2f, halfHeight = h / 2f;
                float ratioSquare = w * w / (float)(h * h);
                float normalizer = MathF.PI / (a1 - a0);
                float op;

                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                        {
                            // Calculate coordinates with the origin at the center
                            xShifted = x / 3f - halfWidth;
                            yShifted = y - halfHeight;
                            // Calculate the radius (A) of the ellipse on which the point is
                            ellipseRadius = MathF.Sqrt(xShifted * xShifted + yShifted * yShifted * ratioSquare);
                            // If the point is outside the vignette area, set the color to black
                            if (ellipseRadius > a1) row[x] = row[x + 1] = row[x + 2] = 0;
                            // Else if the point is outside the clear zone, calculate opacity
                            else if (ellipseRadius >= a0)
                            {
                                op = MathF.Cos((ellipseRadius - a0) * normalizer) / 2f + 0.5f; // Cosine transition
                                //op = 1- (a_tmp - a0) / (a1 - a0); // Linear transition
                                row[x] = (byte)(row[x] * op);
                                row[x + 1] = (byte)(row[x + 1] * op);
                                row[x + 2] = (byte)(row[x + 2] * op);
                            }
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
