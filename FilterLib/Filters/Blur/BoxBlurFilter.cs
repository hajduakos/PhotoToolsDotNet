using FilterLib.Reporting;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Apply blur both horizontally and vertically, uniformly within a radius.
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
        /// Constructor.
        /// </summary>
        /// <param name="radiusX">Horizontal radius [0;...]</param>
        /// <param name="radiusY">Vertical radius [0;...]</param>
        public BoxBlurFilter(int radiusX = 0, int radiusY = 0)
        {
            RadiusX = radiusX;
            RadiusY = radiusY;
        }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // We do the blurring in 2 steps: first horizontal, then vertical
            Image tmp = new(image.Width, image.Height); // Intermediate result between the 2 steps
            System.Diagnostics.Debug.Assert(image.Width == tmp.Width);
            int width_3 = image.Width * 3;
            int radiusX_3 = radiusX * 3;
            fixed (byte* imgStart = image, tmpStart = tmp)
            {
                // Horizontal blur, result is in 'tmp'
                // For each pixel, we have to calculate the average over a given radius.
                // To make this more efficient, a rolling window is used
                for (int y = 0; y < image.Height; ++y)
                {
                    byte* imgRow = imgStart + (y * width_3);
                    byte* tmpRow = tmpStart + (y * width_3);

                    // First fill the window
                    int rSum = 0, gSum = 0, bSum = 0, n = 0;
                    for (int x = 0; x < width_3 && n <= radiusX; x += 3)
                    {
                        rSum += imgRow[x];
                        gSum += imgRow[x + 1];
                        bSum += imgRow[x + 2];
                        ++n;
                    }
                    // First element is the average
                    tmpRow[0] = (byte)(rSum / n);
                    tmpRow[1] = (byte)(gSum / n);
                    tmpRow[2] = (byte)(bSum / n);

                    // Iterate through the other columns
                    for (int x = 3; x < width_3; x += 3)
                    {
                        // Add element to right
                        if (x / 3 + radiusX < image.Width)
                        {
                            rSum += imgRow[x + radiusX_3];
                            gSum += imgRow[x + radiusX_3 + 1];
                            bSum += imgRow[x + radiusX_3 + 2];
                            ++n;
                        }
                        // Remove an element from the left
                        if (x / 3 - radiusX - 1 >= 0)
                        {
                            rSum -= imgRow[x - radiusX_3 - 3];
                            gSum -= imgRow[x - radiusX_3 - 3 + 1];
                            bSum -= imgRow[x - radiusX_3 - 3 + 2];
                            --n;
                        }
                        // The actual element is the average  
                        tmpRow[x] = (byte)(rSum / n);
                        tmpRow[x + 1] = (byte)(gSum / n);
                        tmpRow[x + 2] = (byte)(bSum / n);
                    }
                    // Report progress from 0% to 50%
                    reporter?.Report(y, 0, image.Height * 2 - 1);
                }

                // Vertical blur, the result is in 'image'
                int radiusRowOffset = radiusY * width_3;
                for (int x = 0; x < width_3; x += 3)
                {
                    byte* imgCol = imgStart + x;
                    byte* tmpCol = tmpStart + x;

                    // First fill the window
                    int rSum = 0, gSum = 0, bSum = 0, n = 0;
                    for (int y = 0; y < image.Height && n <= radiusY; ++y)
                    {
                        rSum += tmpCol[y * width_3];
                        gSum += tmpCol[y * width_3 + 1];
                        bSum += tmpCol[y * width_3 + 2];
                        ++n;
                    }
                    // First element is the average
                    imgCol[0] = (byte)(rSum / n);
                    imgCol[1] = (byte)(gSum / n);
                    imgCol[2] = (byte)(bSum / n);

                    // Iterate through the other rows
                    for (int y = 1; y < image.Height; ++y)
                    {
                        int rowOffset = y * width_3;
                        // Add element to bottom
                        if (y + radiusY < image.Height)
                        {
                            rSum += tmpCol[rowOffset + radiusRowOffset];
                            gSum += tmpCol[rowOffset + radiusRowOffset + 1];
                            bSum += tmpCol[rowOffset + radiusRowOffset + 2];
                            ++n;
                        }

                        // Remove an element from the top
                        if (y - radiusY - 1 >= 0)
                        {
                            rSum -= tmpCol[rowOffset - radiusRowOffset - width_3];
                            gSum -= tmpCol[rowOffset - radiusRowOffset - width_3 + 1];
                            bSum -= tmpCol[rowOffset - radiusRowOffset - width_3 + 2];
                            --n;
                        }
                        // The actual element is the average
                        imgCol[rowOffset] = (byte)(rSum / n);
                        imgCol[rowOffset + 1] = (byte)(gSum / n);
                        imgCol[rowOffset + 2] = (byte)(bSum / n);
                    }
                    // Report progress from 50% to 100%
                    reporter?.Report(x + width_3, 0, width_3 * 2 - 3);
                }
            }
            reporter?.Done();
        }
    }
}
