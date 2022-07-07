using FilterLib.Reporting;
using Math = System.Math;
using MathF = System.MathF;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Apply blur by replacing each pixel with the average weighted
    /// using Gaussian distribution within a given radius.
    /// </summary>
    [Filter]
    public sealed class GaussianBlurFilter : FilterInPlaceBase
    {
        private int radius;

        /// <summary>
        /// Blur radius [0,...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int Radius
        {
            get { return radius; }
            set { radius = Math.Max(0, value); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="radius">Blur radius [0;...]</param>
        public GaussianBlurFilter(int radius = 0) => Radius = radius;

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            if (Radius == 0) { reporter?.Done(); return; }
            // We do the blurring in 2 steps: first horizontal, then vertical
            Image tmp = new(image.Width, image.Height); // Intermediate result between the 2 steps
            System.Diagnostics.Debug.Assert(image.Width == tmp.Width);
            int width_3 = image.Width * 3;
            // Calculate the kernel
            float[] kernel = new float[radius * 2 + 1];
            int r = -radius;
            float sqrt2piR = 1f / (MathF.Sqrt(2 * MathF.PI) * radius);
            float rsquare2 = 1f / (2 * radius * radius);
            float kernelSum = 0;
            for (int i = 0; i < kernel.Length; ++i)
            {
                kernel[i] = sqrt2piR * MathF.Exp(-r * r * rsquare2);
                kernelSum += kernel[i];
                ++r;
            }
            // Normalize the kernel
            for (int i = 0; i < kernel.Length; ++i) kernel[i] /= kernelSum;

            fixed (byte* imgStart = image, tmpStart = tmp)
            {
                // Horizontal blur, the result is in 'tmp'
                for (int y = 0; y < image.Height; ++y)
                {
                    byte* imgRow = imgStart + (y * width_3);
                    byte* tmpRow = tmpStart + (y * width_3);

                    for (int x = 0; x < width_3; x += 3)
                    {
                        int x_div3 = x / 3;
                        float rSum = 0, gSum = 0, bSum = 0;
                        for (r = -radius; r <= radius; ++r)
                        {
                            // Determine index: if we are outside on the left/right, take leftmost/rightmost
                            int idx;
                            if (x_div3 + r < 0) idx = 0;
                            else if (x_div3 + r >= image.Width) idx = width_3 - 3;
                            else idx = x + r * 3;
                            rSum += kernel[r + radius] * imgRow[idx];
                            gSum += kernel[r + radius] * imgRow[idx + 1];
                            bSum += kernel[r + radius] * imgRow[idx + 2];
                        }
                        tmpRow[x] = (byte)rSum;
                        tmpRow[x + 1] = (byte)gSum;
                        tmpRow[x + 2] = (byte)bSum;
                    }
                    // Report progress from 0% to 50%
                    reporter?.Report(y, 0, image.Height * 2 - 1);
                }

                // Vertical blur, result is in 'image'
                for (int x = 0; x < width_3; x += 3)
                {
                    byte* imgCol = imgStart + x;
                    byte* tmpCol = tmpStart + x;

                    for (int y = 0; y < image.Height; ++y)
                    {
                        float rSum = 0, gSum = 0, bSum = 0;
                        int yOffset = y * width_3;
                        for (r = -radius; r <= radius; ++r)
                        {
                            // Determine index: if we are outside on the top/bottom, take topmost/bottommost
                            int idx;
                            if (y + r < 0) idx = 0;
                            else if (y + r >= image.Height) idx = (image.Height - 1) * width_3;
                            else idx = (y + r) * width_3;

                            rSum += kernel[r + radius] * tmpCol[idx];
                            gSum += kernel[r + radius] * tmpCol[idx + 1];
                            bSum += kernel[r + radius] * tmpCol[idx + 2];
                        }
                        imgCol[yOffset] = (byte)rSum;
                        imgCol[yOffset + 1] = (byte)gSum;
                        imgCol[yOffset + 2] = (byte)bSum;
                    }
                    // Report progress from 50% to 100%
                    reporter?.Report(x + width_3, 0, width_3 * 2 - 3);
                }
            }
            reporter?.Done();
        }
    }
}
