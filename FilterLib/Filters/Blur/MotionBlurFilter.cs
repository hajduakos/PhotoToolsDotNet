﻿using FilterLib.Reporting;
using FilterLib.Util;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Motion blur filter.
    /// </summary>
    [Filter]
    public sealed class MotionBlurFilter : FilterInPlaceBase
    {
        private int length;

        /// <summary>
        /// Blur length [0;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int Length
        {
            get { return length; }
            set { length = Math.Max(0, value); }
        }

        /// <summary>
        /// Blur angle in degrees [0;360].
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        [FilterParamMaxF(360)]
        public float Angle { get; set; }

        /// <summary>
        /// Constructor with blur lenght and angle
        /// </summary>
        /// <param name="length">Blur length</param>
        /// <param name="angle">Blur angle in degrees</param>
        public MotionBlurFilter(int length = 0, float angle = 0f)
        {
            this.Angle = angle;
            this.Length = length;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            using (Bitmap original = (Bitmap)image.Clone())
            using (DisposableBitmapData bmd = new DisposableBitmapData(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdOrg = new DisposableBitmapData(original, PixelFormat.Format24bppRgb))
            {
                int wMul3 = image.Width * 3;
                int w = image.Width, h = image.Height;
                int stride = bmd.Stride;
                int x, y, dx, dy, k;
                float sinAngle = (float)Math.Sin(Angle * Math.PI / 180);
                float cosAngle = (float)Math.Cos(Angle * Math.PI / 180);
                int rSum, gSum, bSum, n;
                int xAct, yAct, idx;
                unsafe
                {
                    byte* bmdstart = (byte*)bmd.Scan0;
                    byte* orgstart = (byte*)bmdOrg.Scan0;
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                        {
                            // We have to sum each component through a line
                            bSum = gSum = rSum = n = 0;
                            // Iterate through each pixel of the line
                            for (k = -length; k <= length; ++k)
                            {
                                dx = (int)Math.Round(k * cosAngle); // Horizontal distance from the center pixel
                                dy = (int)Math.Round(k * sinAngle); // Vertical distance from the center pixel
                                xAct = x / 3 + dx; // x coord. of the actual pixel
                                yAct = y + dy; // y coord. of the actual pixel
                                // If the pixel is in the bounds of the image
                                if (xAct >= 0 && xAct < w && yAct >= 0 && yAct < h)
                                {
                                    idx = yAct * stride + xAct * 3;   // Calculate index
                                    rSum += orgstart[idx + 2]; // Sum red component
                                    gSum += orgstart[idx + 1]; // Sum blue component
                                    bSum += orgstart[idx];     // Sum green component
                                    ++n; // Number of items
                                }
                            }
                            idx = y * stride + x; // Index of the center pixel
                            // Calculate average
                            bmdstart[idx + 2] = (byte)(rSum / n);
                            bmdstart[idx + 1] = (byte)(gSum / n);
                            bmdstart[idx] = (byte)(bSum / n);
                        }


                        if ((y & 31) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}