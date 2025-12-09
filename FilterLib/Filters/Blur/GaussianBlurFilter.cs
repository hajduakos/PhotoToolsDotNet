using FilterLib.Reporting;
using Math = System.Math;
using MathF = System.MathF;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Blur
{
    [Filter("Blur by replacing each pixel with the Gaussian average of the surrounding rectangle of a given size.")]
    public sealed class GaussianBlurFilter : FilterInPlaceBase
    {
        /// <summary>
        /// Blur radius [0,...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int Radius
        {
            get;
            set { field = Math.Max(0, value); }
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
            object reporterLock = new();
            int progress = 0;
            if (Radius == 0) { reporter?.Done(); return; }
            // We do the blurring in 2 steps: first horizontal, then vertical
            Image tmp = new(image.Width, image.Height); // Intermediate result between the 2 steps
            System.Diagnostics.Debug.Assert(image.Width == tmp.Width);
            int width_3 = image.Width * 3;
            // Calculate the kernel
            float[] kernel = new float[Radius * 2 + 1];
            int r0 = -Radius;
            float sqrt2piR = 1f / (MathF.Sqrt(2 * MathF.PI) * Radius);
            float rsquare2 = 1f / (2 * Radius * Radius);
            float kernelSum = 0;
            for (int i = 0; i < kernel.Length; ++i)
            {
                kernel[i] = sqrt2piR * MathF.Exp(-r0 * r0 * rsquare2);
                kernelSum += kernel[i];
                ++r0;
            }
            // Normalize the kernel
            Parallel.For(0, kernel.Length, i => kernel[i] /= kernelSum);

            fixed (byte* imgStart = image, tmpStart = tmp)
            {
                byte* imgStart0 = imgStart;
                byte* tmpStart0 = tmpStart;
                // Horizontal blur, the result is in 'tmp'
                Parallel.For(0, image.Height, y =>
                {
                    byte* imgRow = imgStart0 + y * width_3;
                    byte* tmpRow = tmpStart0 + y * width_3;

                    for (int x = 0; x < width_3; x += 3)
                    {
                        int x_div3 = x / 3;
                        float rSum = 0, gSum = 0, bSum = 0;
                        for (int r = -Radius; r <= Radius; ++r)
                        {
                            // Determine index: if we are outside on the left/right, take leftmost/rightmost
                            int idx;
                            if (x_div3 + r < 0) idx = 0;
                            else if (x_div3 + r >= image.Width) idx = width_3 - 3;
                            else idx = x + r * 3;
                            rSum += kernel[r + Radius] * imgRow[idx];
                            gSum += kernel[r + Radius] * imgRow[idx + 1];
                            bSum += kernel[r + Radius] * imgRow[idx + 2];
                        }
                        tmpRow[x] = (byte)rSum;
                        tmpRow[x + 1] = (byte)gSum;
                        tmpRow[x + 2] = (byte)bSum;
                    }
                    // Report progress from 0% to 50%
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height * 2);
                });

                progress = 0;
                // Vertical blur, result is in 'image'
                Parallel.For(0, image.Width, x =>
                {
                    x *= 3;
                    byte* imgCol = imgStart0 + x;
                    byte* tmpCol = tmpStart0 + x;

                    for (int y = 0; y < image.Height; ++y)
                    {
                        float rSum = 0, gSum = 0, bSum = 0;
                        int yOffset = y * width_3;
                        for (int r = -Radius; r <= Radius; ++r)
                        {
                            // Determine index: if we are outside on the top/bottom, take topmost/bottommost
                            int idx;
                            if (y + r < 0) idx = 0;
                            else if (y + r >= image.Height) idx = (image.Height - 1) * width_3;
                            else idx = (y + r) * width_3;

                            rSum += kernel[r + Radius] * tmpCol[idx];
                            gSum += kernel[r + Radius] * tmpCol[idx + 1];
                            bSum += kernel[r + Radius] * tmpCol[idx + 2];
                        }
                        imgCol[yOffset] = (byte)rSum;
                        imgCol[yOffset + 1] = (byte)gSum;
                        imgCol[yOffset + 2] = (byte)bSum;
                    }
                    // Report progress from 50% to 100%
                    if (reporter != null) lock (reporterLock) reporter.Report(image.Width + ++progress, 0, image.Width * 2);
                });

            }
            reporter?.Done();
        }
    }
}
