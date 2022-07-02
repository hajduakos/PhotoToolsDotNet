﻿using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;
using MathF = System.MathF;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Gaussian blur.
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
        /// Constructor
        /// </summary>
        /// <param name="radius">Blur radius [0;...]</param>
        public GaussianBlurFilter(int radius = 0) => Radius = radius;

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            if (Radius == 0) { reporter?.Done(); return; }
            // Clone image for temporary use
            using (Bitmap tmp = (Bitmap)image.Clone())
            // Lock bits
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdTmp = new(tmp, PixelFormat.Format24bppRgb))
            {
                int width_3 = image.Width * 3;
                float sumR, sumG, sumB;

                // Calculate the kernel
                float[] kernel = new float[radius * 2 + 1];
                int r = -radius;
                float sqrt2piR = 1f / (MathF.Sqrt(2 * MathF.PI) * radius);
                float rsquare2 = 1f / (2 * radius * radius);
                float kernelSum = 0;
                for (int x = 0; x < kernel.Length; x++)
                {
                    kernel[x] = sqrt2piR * MathF.Exp(-r * r * rsquare2);
                    kernelSum += kernel[x];
                    ++r;
                }
                // Normalize the kernel
                for (int x = 0; x < kernel.Length; x++)
                    kernel[x] /= kernelSum;

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

                        // Iterate through each column
                        for (int x = 0; x < width_3; x += 3)
                        {
                            int x_div3 = x / 3;
                            sumR = sumG = sumB = 0; // Clear sum
                            // Iterate through the kernel
                            for (r = -radius; r <= radius; ++r)
                            {
                                int kIdx = r + radius; // Kernel indexer
                                int idx;
                                if (x_div3 + r < 0) idx = 0; // If we are outside the image at the left, take the leftmost pixel
                                else if (x_div3 + r >= image.Width) idx = width_3 - 3;  // If we are outside the image at the right, take the rightmost pixel
                                else idx = x + r * 3; // Else take the actual pixel
                                // Add to the sum
                                sumB += kernel[kIdx] * row[idx];
                                sumG += kernel[kIdx] * row[idx + 1];
                                sumR += kernel[kIdx] * row[idx + 2];
                            }
                            rowTmp[x] = (byte)sumB;
                            rowTmp[x + 1] = (byte)sumG;
                            rowTmp[x + 2] = (byte)sumR;
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

                        // Iterate through the other rows
                        for (int y = 0; y < image.Height; ++y)
                        {
                            sumR = sumG = sumB = 0; // Clear sum
                            int y_stride = y * bmd.Stride;
                            // Iterate through the kernel
                            for (r = -radius; r <= radius; ++r)
                            {
                                int kIdx = r + radius; // Kernel indexer
                                int idx;
                                if (y + r < 0) idx = 0; // If we are outside the image at the top, take the topmost pixel
                                else if (y + r >= image.Height) idx = (image.Height - 1) * bmd.Stride; // If we are outside the image at the bottom, take the bottom pixel
                                else idx = (y + r) * bmd.Stride;// Else take the actual pixel

                                // Add to the sum
                                sumB += kernel[kIdx] * colTmp[idx];
                                sumG += kernel[kIdx] * colTmp[idx + 1];
                                sumR += kernel[kIdx] * colTmp[idx + 2];
                            }
                            col[y_stride] = (byte)sumB;
                            col[y_stride + 1] = (byte)sumG;
                            col[y_stride + 2] = (byte)sumR;
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
