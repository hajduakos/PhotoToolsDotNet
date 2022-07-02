using FilterLib.Reporting;
using FilterLib.Util;
using System;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Vignette filter.
    /// </summary>
    [Filter]
    public sealed class VignetteFilter : FilterInPlaceBase
    {

        /// <summary>
        /// Radius of the vignette, outside is filled with given color.
        /// </summary>
        [FilterParam]
        public Util.Size Radius { get; set; }

        /// <summary>
        /// Radius of the clear zone.
        /// </summary>
        [FilterParam]
        public Util.Size ClearRadius { get; set; }

        /// <summary>
        /// Vignette color.
        /// </summary>
        [FilterParam]
        public RGB Color { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public VignetteFilter() : this(Util.Size.Relative(3), Util.Size.Relative(2), new RGB(0, 0, 0)) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="radius">Radius of the vignette</param>
        /// <param name="clearRadius">Radius of the clear zone</param>
        /// <param name="color">Color of the vignette</param>
        public VignetteFilter(Util.Size radius, Util.Size clearRadius, RGB color)
        {
            Radius = radius;
            ClearRadius = clearRadius;
            Color = color;
        }

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                int width_3 = image.Width * 3;
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
                        for (x = 0; x < width_3; x += 3)
                        {
                            // Calculate coordinates with the origin at the center
                            xShifted = x / 3f - halfWidth;
                            yShifted = y - halfHeight;
                            // Calculate the radius (A) of the ellipse on which the point is
                            ellipseRadius = MathF.Sqrt(xShifted * xShifted + yShifted * yShifted * ratioSquare);
                            // If the point is outside the vignette area, set the color to given one
                            if (ellipseRadius > a1)
                            {
                                row[x] = (byte)Color.B;
                                row[x + 1] = (byte)Color.G;
                                row[x + 2] = (byte)Color.R;
                            }
                            // Else if the point is outside the clear zone, calculate opacity
                            else if (ellipseRadius >= a0)
                            {
                                op = MathF.Cos((ellipseRadius - a0) * normalizer) / 2f + 0.5f; // Cosine transition
                                //op = 1- (a_tmp - a0) / (a1 - a0); // Linear transition
                                row[x] = (byte)(row[x] * op + Color.B * (1 - op));
                                row[x + 1] = (byte)(row[x + 1] * op + Color.G * (1 - op));
                                row[x + 2] = (byte)(row[x + 2] * op + Color.R * (1 - op));
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
