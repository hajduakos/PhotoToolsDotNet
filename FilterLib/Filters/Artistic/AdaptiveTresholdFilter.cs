﻿using FilterLib.Reporting;

namespace FilterLib.Filters.Artistic
{
    /// <summary>
    /// Map each pixel to black or white based on an adaptive treshold: the average
    /// intensity is calculated in a given radius and used as a treshold.
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
        /// Constructor.
        /// </summary>
        /// <param name="squareSize">Square size [1;...]</param>
        public AdaptiveTresholdFilter(int squareSize = 1) => SquareSize = squareSize;

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();

            fixed (byte* newStart = image, oldStart = original)
            {
                System.Diagnostics.Debug.Assert(image.Width == original.Width);
                int width_3 = image.Width * 3;
                int sqSize_3 = sqSize * 3;

                // For each pixel we have to calculate the average intensity in a given radius. To make this
                // more efficient, we go row-by-row and use a moving window:
                // - First we calculate the full window for the first pixel
                // - But then in each step moving right, we drop one row from the left and add one at the right
                for (int y = 0; y < image.Height; ++y)
                {
                    byte* newRow = newStart + (y * width_3);
                    byte* oldRow = oldStart + (y * width_3);

                    float sum = 0;
                    int n = 0;

                    // Calculate full window around first column of current row
                    for (int xSub = 0; xSub <= sqSize && xSub < image.Width; ++xSub)
                    {
                        for (int ySub = y < sqSize ? -y : -sqSize; y + ySub < image.Height && ySub <= sqSize; ++ySub)
                        {
                            int idx = ySub * width_3 + xSub * 3;
                            sum += RRatio * oldRow[idx] + GRatio * oldRow[idx + 1] + BRatio * oldRow[idx + 2];
                            ++n;
                        }
                    }
                    float avg = sum / n;
                    // Treshold first column of current row
                    float lum = RRatio * oldRow[0] + GRatio * oldRow[1] + BRatio * oldRow[2];
                    newRow[0] = newRow[1] = newRow[2] = (byte)(avg < lum ? 255 : 0);

                    // Iterate through other columns and update window
                    for (int x = 3; x < width_3; x += 3)
                    {
                        // Remove a column from the left
                        if (x / 3 - sqSize - 1 >= 0)
                        {
                            for (int ySub = y < sqSize ? -y : -sqSize; y + ySub < image.Height && ySub <= sqSize; ++ySub)
                            {
                                int idx = ySub * width_3 + x - sqSize_3 - 3;
                                sum -= RRatio * oldRow[idx] + GRatio * oldRow[idx + 1] + BRatio * oldRow[idx + 2];
                                --n;
                            }
                        }
                        // Add column to right
                        if (x / 3 + sqSize < image.Width)
                        {
                            for (int ySub = y < sqSize ? -y : -sqSize; y + ySub < image.Height && ySub <= sqSize; ++ySub)
                            {
                                int idx = ySub * width_3 + x + sqSize_3;
                                sum += RRatio * oldRow[idx] + GRatio * oldRow[idx + 1] + BRatio * oldRow[idx + 2];
                                ++n;
                            }
                        }
                        avg = sum / n;
                        // Treshold element
                        lum = RRatio * oldRow[x] + GRatio * oldRow[x + 1] + BRatio * oldRow[x + 2];
                        newRow[x] = newRow[x + 1] = newRow[x + 2] = (byte)(avg < lum ? 255 : 0);
                    }
                    reporter?.Report(y, 0, image.Height - 1);
                }
            }

            reporter?.Done();
        }
    }
}
