using FilterLib.Reporting;
using FilterLib.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Box blur filter.
    /// </summary>
    [Filter]
    public sealed class BoxBlurFilter : FilterInPlaceBase
    {
        private int radiusX, radiusY;

        /// <summary>
        /// Horizontal radius [0;...]
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int RadiusX
        {
            get { return radiusX; }
            set { radiusX = System.Math.Max(0, value); }
        }

        /// <summary>
        /// Vertical radius [0;...]
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int RadiusY
        {
            get { return radiusY; }
            set { radiusY = System.Math.Max(0, value); }
        }

        /// <summary>
        /// Constructor with horizontal and vertical radius parameters </summary>
        /// <param name="radiusX">Horizontal radius [0;...]</param>
        /// <param name="radiusY">Vertical radius [0;...]</param>
        public BoxBlurFilter(int radiusX = 0, int radiusY = 0)
        {
            RadiusX = radiusX;
            RadiusY = radiusY;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image for temporary use
            using (Bitmap tmp = (Bitmap)image.Clone())
            // Lock bits
            using (DisposableBitmapData bmd = new DisposableBitmapData(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdTmp = new DisposableBitmapData(tmp, PixelFormat.Format24bppRgb))
            {
                int x, y, rSum, gSum, bSum, n;
                int w = image.Width, h = image.Height;
                int wMul3 = image.Width * 3;
                int stride = bmd.Stride;
                int radiusXmul3 = radiusX * 3;

                unsafe
                {
                    // We do the blurring in 2 steps: first horizontal, then vertical

                    // First iterate through rows and do horizontal blur
                    // The result is in 'tmp'
                    for (y = 0; y < h; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmd.Scan0 + (y * stride);
                        byte* rowTmp = (byte*)bmdTmp.Scan0 + (y * stride);

                        // Clear sums
                        rSum = gSum = bSum = n = 0;
                        //Take the sum of the first rx+1 elements
                        for (x = 0; x < wMul3 && n <= radiusX; x += 3)
                        {
                            rSum += row[x + 2];
                            gSum += row[x + 1];
                            bSum += row[x];
                            ++n;
                        }
                        // First element is the average
                        rowTmp[2] = (byte)(rSum / n);
                        rowTmp[1] = (byte)(gSum / n);
                        rowTmp[0] = (byte)(bSum / n);

                        // Iterate through the other columns
                        for (x = 3; x < wMul3; x += 3)
                        {
                            // If we can take a new element from the right
                            if (x / 3 + radiusX < w)
                            {
                                rSum += row[x + radiusXmul3 + 2];
                                gSum += row[x + radiusXmul3 + 1];
                                bSum += row[x + radiusXmul3];
                                ++n;
                            }
                            // If we can remove an element from the left
                            if (x / 3 - radiusX - 1 >= 0)
                            {
                                rSum -= row[x - radiusXmul3 - 3 + 2];
                                gSum -= row[x - radiusXmul3 - 3 + 1];
                                bSum -= row[x - radiusXmul3 - 3];
                                --n;
                            }
                            // The actual element is the average  
                            rowTmp[x + 2] = (byte)(rSum / n);
                            rowTmp[x + 1] = (byte)(gSum / n);
                            rowTmp[x] = (byte)(bSum / n);
                        }
                        // Report progress from 0% to 50%
                        if ((y & 63) == 0) reporter?.Report(y, 0, h * 2 - 1);
                    }

                    // Then iterate through columns and do vertical blur
                    // The result is in 'image'
                    for (x = 0; x < wMul3; x += 3)
                    {
                        // Get columns
                        byte* col = (byte*)bmd.Scan0 + x;
                        byte* colTmp = (byte*)bmdTmp.Scan0 + x;

                        // Clear sums
                        rSum = gSum = bSum = n = 0;

                        // Take the sum of the first ry+1 elements
                        for (y = 0; y < h && n <= radiusY; ++y)
                        {
                            rSum += colTmp[y * stride + 2];
                            gSum += colTmp[y * stride + 1];
                            bSum += colTmp[y * stride];
                            ++n;
                        }
                        // First element will be the average
                        col[2] = (byte)(rSum / n);
                        col[1] = (byte)(gSum / n);
                        col[0] = (byte)(bSum / n);

                        // Iterate through the other rows
                        for (y = 1; y < h; ++y)
                        {
                            int j_stride = y * stride;
                            int ry_stride = radiusY * stride;
                            // If we can take a new element from the bottom
                            if (y + radiusY < h)
                            {
                                rSum += colTmp[j_stride + ry_stride + 2];
                                gSum += colTmp[j_stride + ry_stride + 1];
                                bSum += colTmp[j_stride + ry_stride];
                                ++n;
                            }

                            // If we can remove an element from the top
                            if (y - radiusY - 1 >= 0)
                            {
                                rSum -= colTmp[j_stride - ry_stride - stride + 2]; // Row (j-ry-1)
                                gSum -= colTmp[j_stride - ry_stride - stride + 1];
                                bSum -= colTmp[j_stride - ry_stride - stride];
                                --n;
                            }
                            // The actual element will be the average
                            col[j_stride + 2] = (byte)(rSum / n);
                            col[j_stride + 1] = (byte)(gSum / n);
                            col[j_stride] = (byte)(bSum / n);
                        }
                        // Report progress from 50% to 100%
                        if ((x & 63) == 0) reporter?.Report(x + wMul3, 0, wMul3 * 2 - 3);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
