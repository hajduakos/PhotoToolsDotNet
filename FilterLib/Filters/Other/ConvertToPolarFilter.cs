using FilterLib.Reporting;
using FilterLib.Util;
using System.Drawing.Imaging;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;

namespace FilterLib.Filters.Other
{
    /// <summary>
    /// Convert to polar filter.
    /// </summary>
    [Filter]
    public sealed class ConvertToPolarFilter : FilterInPlaceBase
    {
        private float phase;

        /// <summary>
        /// Phase [0;360].
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        [FilterParamMaxF(360)]
        public float Phase
        {
            get { return phase; }
            set
            {
                phase = value;
                while (phase > 360) phase -= 360;
                while (phase < 0) phase += 360;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="phase">Phase [0;360]</param>
        public ConvertToPolarFilter(float phase = 0)
        {
            this.Phase = phase;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            using (Bitmap original = (Bitmap)image.Clone())
            using (DisposableBitmapData bmd = new DisposableBitmapData(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdOrg = new DisposableBitmapData(original, PixelFormat.Format24bppRgb))
            {
                int w = image.Width, h = image.Height;
                int stride = bmd.Stride;
                int wMul3 = image.Width * 3; // Width of a row
                int x, y;
                float xCorr, yCorr; // Corrected coordinates
                float halfHeight = (h - 1) / 2f;
                float halfWidth = (w - 1) / 2f;
                float r, fi; // Radius and angle
                float xMult = h / (float)w; // Correction to circle
                int x0, y0, x1, y1; // Indexes in the original image (bilinear interpolation with 4 points)
                float xOrg, yOrg; // Coordinates in the original image (float)
                float xFrac, yFrac; // Fractional parts of the coordinates
                float xMultiplier = (float)(1 / (2 * Math.PI) * (w - 1)); // Multiplier to convert to polar
                float yMultiplier = 1 / halfHeight * (h - 1); // Multiplier to convert to polar

                unsafe
                {
                    byte* orgStart = (byte*)bmdOrg.Scan0;
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmd.Scan0 + (y * stride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                        {
                            // Get corrected coordinates
                            xCorr = ((x / 3) - halfWidth) * xMult;
                            yCorr = halfHeight - y;
                            // Get radius and angle
                            r = (float)Math.Sqrt(xCorr * xCorr + yCorr * yCorr);
                            fi = (float)(-Math.Atan2(xCorr, yCorr) + Math.PI + phase / 180f * Math.PI);

                            if (fi >= 2 * Math.PI) fi -= (float)(2 * Math.PI);

                            // Get X coordinate and points for interpolation in the original image
                            xOrg = fi * xMultiplier;

                            x0 = (int)Math.Floor(xOrg);
                            xFrac = xOrg - x0;
                            if (x0 >= w) x0 = w - 1;
                            x0 *= 3;

                            x1 = (int)Math.Ceiling(xOrg);
                            if (x1 >= w) x1 = w - 1;
                            x1 *= 3;

                            // Get Y coordinate and points for interpolation in the original image
                            yOrg = r * yMultiplier;

                            y0 = (int)Math.Floor(yOrg);
                            yFrac = yOrg - y0;

                            if (y0 >= h) y0 = h - 1;

                            y1 = (int)Math.Ceiling(yOrg);
                            if (y1 >= h) y1 = h - 1;

                            // Interpolation for R,G,B components
                            row[x] = (byte)(orgStart[y0 * stride + x0] * (1 - xFrac) * (1 - yFrac)
                                + orgStart[y1 * stride + x0] * (1 - xFrac) * yFrac
                                + orgStart[y0 * stride + x1] * xFrac * (1 - yFrac)
                                + orgStart[y1 * stride + x1] * xFrac * yFrac);
                            row[x + 1] = (byte)(orgStart[y0 * stride + x0 + 1] * (1 - xFrac) * (1 - yFrac)
                                + orgStart[y1 * stride + x0 + 1] * (1 - xFrac) * yFrac
                                + orgStart[y0 * stride + x1 + 1] * xFrac * (1 - yFrac)
                                + orgStart[y1 * stride + x1 + 1] * xFrac * yFrac);
                            row[x + 2] = (byte)(orgStart[y0 * stride + x0 + 2] * (1 - xFrac) * (1 - yFrac)
                                + orgStart[y1 * stride + x0 + 2] * (1 - xFrac) * yFrac
                                + orgStart[y0 * stride + x1 + 2] * xFrac * (1 - yFrac)
                                + orgStart[y1 * stride + x1 + 2] * xFrac * yFrac);
                        }
                        if (reporter != null && ((y & 63) == 0)) reporter.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
