using System.Drawing;
using System.Drawing.Imaging;
using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Artistic
{
    /// <summary>
    /// Adaptive treshold filter.
    /// </summary>
    [Filter]
    public sealed class AdaptiveTresholdFilter : FilterInPlaceBase
    {
        private const float RRatio = 0.299f;
        private const float GRatio = 0.587f;
        private const float BRatio = 0.114f;
        private int sqSize;

        /// <summary>
        /// Square size [1;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(1)]
        public int SquareSize
        {
            get { return sqSize; }
            set { sqSize = System.Math.Max(1, value); }
        }

        /// <summary>
        /// Constructor with square size.
        /// </summary>
        /// <param name="squareSize">Square size</param>
        public AdaptiveTresholdFilter(int squareSize = 1)
        {
            this.SquareSize = squareSize;
        }

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
                int squareSizeMul3 = sqSize * 3;
                int x, y, xSub, ySub, idx, n;
                float sum, lum, avg;

                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmd.Scan0 + (y * stride);
                        byte* rowOrg = (byte*)bmdOrg.Scan0 + (y * stride);

                        // Clear sum
                        sum = n = 0;

                        // Calculate sum with the surrounding of the first column of the actual row
                        for (xSub = 0; xSub <= sqSize && xSub < w; ++xSub) // Horizontal
                        {
                            // Vertically from -squareSize to +squareSize
                            for (ySub = y < sqSize ? -y : -sqSize; y + ySub < h && ySub <= sqSize; ++ySub)
                            {
                                // Calculate index (relative to current row)
                                idx = ySub * stride + xSub * 3;
                                // Add luminance
                                sum += RRatio * rowOrg[idx + 2] + GRatio * rowOrg[idx + 1] + BRatio * rowOrg[idx];
                                ++n;
                            }
                        }
                        // Get average and luminance
                        avg = sum / n;
                        lum = RRatio * rowOrg[2] + GRatio * rowOrg[1] + BRatio * rowOrg[0];
                        // Treshold first element
                        row[0] = row[1] = row[2] = (byte)(avg < lum ? 255 : 0);

                        // Iterate through other columns
                        // Rather than calculating the sum again, we just take in another column
                        // (+squareSize) and remove a column (-squareSize-1)
                        for (x = 3; x < wMul3; x += 3)
                        {
                            // If we can remove a column
                            if (x / 3 - sqSize - 1 >= 0)
                            {
                                // Remove column (vertically from -squareSize to +squareSize)
                                for (ySub = y < sqSize ? -y : -sqSize; y + ySub < h && ySub <= sqSize; ++ySub)
                                {
                                    // Calculate index (relative to current row)
                                    idx = ySub * stride + x - squareSizeMul3 - 3;
                                    // Subtract luminance
                                    sum -= RRatio * rowOrg[idx + 2] + GRatio * rowOrg[idx + 1] + BRatio * rowOrg[idx];
                                    --n;
                                }
                            }
                            // If we can add a new column
                            if (x / 3 + sqSize < w)
                            {
                                // Add column (vertically from -squareSize to +squareSize)
                                for (ySub = y < sqSize ? -y : -sqSize; y + ySub < h && ySub <= sqSize; ++ySub)
                                {
                                    // Calculate index (relative to current row)
                                    idx = ySub * stride + x + squareSizeMul3;
                                    // Add luminance
                                    sum += RRatio * rowOrg[idx + 2] + GRatio * rowOrg[idx + 1] + BRatio * rowOrg[idx];
                                    ++n;
                                }
                            }
                            // Get average and luminance
                            avg = sum / n;
                            lum = RRatio * rowOrg[x + 2] + GRatio * rowOrg[x + 1] + BRatio * rowOrg[x];
                            // Treshold element
                            row[x] = row[x + 1] = row[x + 2] = (byte)(avg < lum ? 255 : 0);
                        }
                        if (reporter != null && ((y & 63) == 0)) reporter.Report(y, 0, h - 1);
                    }
                }
            }
            if (reporter != null) reporter.Done();
        }
    }
}
