using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

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
        public AdaptiveTresholdFilter(int squareSize = 1) => SquareSize = squareSize;

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            using (Bitmap original = (Bitmap)image.Clone())
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdOrig = new(original, PixelFormat.Format24bppRgb))
            {
                int width_3 = image.Width * 3;
                int sqSize_3 = sqSize * 3;

                unsafe
                {
                    // Iterate through rows
                    for (int y = 0; y < image.Height; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        byte* rowOrig = (byte*)bmdOrig.Scan0 + (y * bmdOrig.Stride);

                        // Clear sum
                        float sum = 0;
                        int n = 0;

                        // Calculate sum with the surrounding of the first column of the actual row
                        for (int xSub = 0; xSub <= sqSize && xSub < image.Width; ++xSub) // Horizontal
                        {
                            // Vertically from -squareSize to +squareSize
                            for (int ySub = y < sqSize ? -y : -sqSize; y + ySub < image.Height && ySub <= sqSize; ++ySub)
                            {
                                // Calculate index (relative to current row)
                                int idx = ySub * bmdOrig.Stride + xSub * 3;
                                // Add luminance
                                sum += RRatio * rowOrig[idx + 2] + GRatio * rowOrig[idx + 1] + BRatio * rowOrig[idx];
                                ++n;
                            }
                        }
                        // Get average and luminance
                        float avg = sum / n;
                        float lum = RRatio * rowOrig[2] + GRatio * rowOrig[1] + BRatio * rowOrig[0];
                        // Treshold first element
                        row[0] = row[1] = row[2] = (byte)(avg < lum ? 255 : 0);

                        // Iterate through other columns
                        // Rather than calculating the sum again, we just take in another column
                        // (+squareSize) and remove a column (-squareSize-1)
                        for (int x = 3; x < width_3; x += 3)
                        {
                            // If we can remove a column
                            if (x / 3 - sqSize - 1 >= 0)
                            {
                                // Remove column (vertically from -squareSize to +squareSize)
                                for (int ySub = y < sqSize ? -y : -sqSize; y + ySub < image.Height && ySub <= sqSize; ++ySub)
                                {
                                    // Calculate index (relative to current row)
                                    int idx = ySub * bmdOrig.Stride + x - sqSize_3 - 3;
                                    // Subtract luminance
                                    sum -= RRatio * rowOrig[idx + 2] + GRatio * rowOrig[idx + 1] + BRatio * rowOrig[idx];
                                    --n;
                                }
                            }
                            // If we can add a new column
                            if (x / 3 + sqSize < image.Width)
                            {
                                // Add column (vertically from -squareSize to +squareSize)
                                for (int ySub = y < sqSize ? -y : -sqSize; y + ySub < image.Height && ySub <= sqSize; ++ySub)
                                {
                                    // Calculate index (relative to current row)
                                    int idx = ySub * bmd.Stride + x + sqSize_3;
                                    // Add luminance
                                    sum += RRatio * rowOrig[idx + 2] + GRatio * rowOrig[idx + 1] + BRatio * rowOrig[idx];
                                    ++n;
                                }
                            }
                            // Get average and luminance
                            avg = sum / n;
                            lum = RRatio * rowOrig[x + 2] + GRatio * rowOrig[x + 1] + BRatio * rowOrig[x];
                            // Treshold element
                            row[x] = row[x + 1] = row[x + 2] = (byte)(avg < lum ? 255 : 0);
                        }
                        reporter?.Report(y, 0, image.Height - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
