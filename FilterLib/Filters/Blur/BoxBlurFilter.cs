using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

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

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image for temporary use (intermediate result)
            using (Bitmap tmp = (Bitmap)image.Clone())
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdTmp = new(tmp, PixelFormat.Format24bppRgb))
            {
                int width_3 = image.Width * 3;
                int radiusX_3 = radiusX * 3;

                unsafe
                {
                    // We do the blurring in 2 steps: first horizontal, then vertical

                    // First iterate through rows and do horizontal blur
                    // The result is in 'tmp'
                    for (int y = 0; y < image.Height; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        byte* rowTmp = (byte*)bmdTmp.Scan0 + (y * bmdTmp.Stride);

                        // Clear sums
                        int rSum = 0, gSum = 0, bSum = 0, n = 0;
                        //Take the sum of the first rx+1 elements
                        for (int x = 0; x < width_3 && n <= radiusX; x += 3)
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
                        for (int x = 3; x < width_3; x += 3)
                        {
                            // If we can take a new element from the right
                            if (x / 3 + radiusX < image.Width)
                            {
                                rSum += row[x + radiusX_3 + 2];
                                gSum += row[x + radiusX_3 + 1];
                                bSum += row[x + radiusX_3];
                                ++n;
                            }
                            // If we can remove an element from the left
                            if (x / 3 - radiusX - 1 >= 0)
                            {
                                rSum -= row[x - radiusX_3 - 3 + 2];
                                gSum -= row[x - radiusX_3 - 3 + 1];
                                bSum -= row[x - radiusX_3 - 3];
                                --n;
                            }
                            // The actual element is the average  
                            rowTmp[x + 2] = (byte)(rSum / n);
                            rowTmp[x + 1] = (byte)(gSum / n);
                            rowTmp[x] = (byte)(bSum / n);
                        }
                        // Report progress from 0% to 50%
                        reporter?.Report(y, 0, image.Height * 2 - 1);
                    }

                    // Then iterate through columns and do vertical blur
                    // The result is in 'image'
                    for (int x = 0; x < width_3; x += 3)
                    {
                        // Get columns
                        byte* col = (byte*)bmd.Scan0 + x;
                        byte* colTmp = (byte*)bmdTmp.Scan0 + x;

                        // Clear sums
                        int rSum = 0, gSum = 0, bSum = 0, n = 0;

                        // Take the sum of the first ry+1 elements
                        for (int y = 0; y < image.Height && n <= radiusY; ++y)
                        {
                            rSum += colTmp[y * bmdTmp.Stride + 2];
                            gSum += colTmp[y * bmdTmp.Stride + 1];
                            bSum += colTmp[y * bmdTmp.Stride];
                            ++n;
                        }
                        // First element will be the average
                        col[2] = (byte)(rSum / n);
                        col[1] = (byte)(gSum / n);
                        col[0] = (byte)(bSum / n);

                        // Iterate through the other rows
                        for (int y = 1; y < image.Height; ++y)
                        {
                            int j_stride = y * bmd.Stride;
                            int ry_stride = radiusY * bmd.Stride;
                            // If we can take a new element from the bottom
                            if (y + radiusY < image.Height)
                            {
                                rSum += colTmp[j_stride + ry_stride + 2];
                                gSum += colTmp[j_stride + ry_stride + 1];
                                bSum += colTmp[j_stride + ry_stride];
                                ++n;
                            }

                            // If we can remove an element from the top
                            if (y - radiusY - 1 >= 0)
                            {
                                rSum -= colTmp[j_stride - ry_stride - bmd.Stride + 2]; // Row (j-ry-1)
                                gSum -= colTmp[j_stride - ry_stride - bmd.Stride + 1];
                                bSum -= colTmp[j_stride - ry_stride - bmd.Stride];
                                --n;
                            }
                            // The actual element will be the average
                            col[j_stride + 2] = (byte)(rSum / n);
                            col[j_stride + 1] = (byte)(gSum / n);
                            col[j_stride] = (byte)(bSum / n);
                        }
                        // Report progress from 50% to 100%
                        reporter?.Report(x + width_3, 0, width_3 * 2 - 3);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
