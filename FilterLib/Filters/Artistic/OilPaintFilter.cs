using System.Drawing;
using System.Drawing.Imaging;
using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Artistic
{
    /// <summary>
    /// Oil paint filter.
    /// </summary>
    [Filter]
    public sealed class OilPaintFilter : FilterInPlaceBase
    {
        private int radius;

        /// <summary>
        /// Brush radius [1;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(1)]
        public int Radius
        {
            get { return radius; }
            set { radius = System.Math.Max(1, value); }
        }

        /// <summary>
        /// Constructor with brush radius.
        /// </summary>
        /// <param name="radius">Brush radius</param>
        public OilPaintFilter(int radius = 1)
        {
            this.Radius = radius;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            if (reporter != null) reporter.Start();
            // Clone image (the clone won't be modified)
            using (Bitmap original = (Bitmap)image.Clone())
            using (DisposableBitmapData bmd = new DisposableBitmapData(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdOrg = new DisposableBitmapData(original, PixelFormat.Format24bppRgb))
            {
                int w = image.Width, h = image.Height;
                int stride = bmd.Stride;
                int wMul3 = image.Width * 3;
                int radiusMul3 = radius * 3;
                int x, y, xSub, ySub, avg, idx, max;
                int[] red = new int[256];
                int[] green = new int[256];
                int[] blue = new int[256];
                int[] intensities = new int[256];

                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmd.Scan0 + (y * stride);
                        byte* rowOrg = (byte*)bmdOrg.Scan0 + (y * stride);

                        // Clear arrays
                        for (xSub = 0; xSub < 256; ++xSub) red[xSub] = green[xSub] = blue[xSub] = intensities[xSub] = 0;

                        // Build up arrays with the surrounding of the first column of the actual row
                        // Horizontal
                        for (xSub = 0; xSub <= radius && xSub < w; ++xSub)
                        {
                            // Vertically from -radius to +radius
                            for (ySub = y < radius ? -y : -radius; y + ySub < h && ySub <= radius; ++ySub)
                            {
                                // Calculate index (relative to current row)
                                idx = ySub * stride + xSub * 3;
                                // Get luminance
                                avg = (int)(0.299 * rowOrg[idx + 2] + 0.587 * rowOrg[idx + 1] + 0.114 * rowOrg[idx]);
                                // Increase values
                                intensities[avg]++;
                                // Sum values
                                red[avg] += rowOrg[idx + 2];
                                green[avg] += rowOrg[idx + 1];
                                blue[avg] += rowOrg[idx];
                            }
                        }
                        // Get the most frequent intensity
                        max = 0;
                        for (xSub = 0; xSub < 256; xSub++) if (intensities[max] < intensities[xSub]) max = xSub;
                        // First element of the row will be the average of the maximal intensity
                        row[2] = (byte)(red[max] / intensities[max]);
                        row[1] = (byte)(green[max] / intensities[max]);
                        row[0] = (byte)(blue[max] / intensities[max]);

                        // Iterate through other columns
                        // Rather than building up the whole array again, we just take in another column
                        // (+radius) and remove a column (-radius-1)
                        for (x = 3; x < wMul3; x += 3)
                        {
                            // If we can remove a column
                            if (x / 3 - radius - 1 >= 0)
                            {
                                // Remove column (vertically from -radius to +radius)
                                for (ySub = y < radius ? -y : -radius; y + ySub < h && ySub <= radius; ++ySub)
                                {
                                    // Calculate index (relative to current row)
                                    idx = ySub * stride + x - radiusMul3 - 3;
                                    // Get luminance
                                    avg = (int)(0.299 * rowOrg[idx + 2] + 0.587 * rowOrg[idx + 1] + 0.114 * rowOrg[idx]);
                                    // Decrease values
                                    intensities[avg]--;
                                    // Sum values
                                    red[avg] -= rowOrg[idx + 2];
                                    green[avg] -= rowOrg[idx + 1];
                                    blue[avg] -= rowOrg[idx];
                                }
                            }
                            // If we can add a new column
                            if (x / 3 + radius < w)
                            {
                                // Add column (vertically from -radius to +radius)
                                for (ySub = y < radius ? -y : -radius; y + ySub < h && ySub <= radius; ++ySub)
                                {
                                    // Calculate index (relative to current row)
                                    idx = ySub * stride + x + radiusMul3;
                                    // Get luminance
                                    avg = (int)(0.299 * rowOrg[idx + 2] + 0.587 * rowOrg[idx + 1] + 0.114 * rowOrg[idx]);
                                    // Increase values
                                    intensities[avg]++;
                                    // Sum values
                                    red[avg] += rowOrg[idx + 2];
                                    green[avg] += rowOrg[idx + 1];
                                    blue[avg] += rowOrg[idx];
                                }
                            }
                            // Get the most frequent intensity
                            max = 0;
                            for (xSub = 0; xSub < 256; xSub++) if (intensities[max] < intensities[xSub]) max = xSub;
                            // Actual element will be the average of the maximal intensity
                            row[x + 2] = (byte)(red[max] / intensities[max]);
                            row[x + 1] = (byte)(green[max] / intensities[max]);
                            row[x] = (byte)(blue[max] / intensities[max]);
                        }

                        if (reporter != null && ((y & 31) == 0)) reporter.Report(y, 0, h - 1);
                    }
                }
            }
            if (reporter != null) reporter.Done();
        }
    }
}
