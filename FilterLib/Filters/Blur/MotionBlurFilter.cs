﻿using FilterLib.Reporting;
using Math = System.Math;
using MathF = System.MathF;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Apply motion blur by replacing each pixel with the averag
    /// calculated along a line with a given angle and length.
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
        /// Constructor.
        /// </summary>
        /// <param name="length">Blur length</param>
        /// <param name="angle">Blur angle in degrees</param>
        public MotionBlurFilter(int length = 0, float angle = 0f)
        {
            Angle = angle;
            Length = length;
        }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(image.Width == original.Width);
            int width_3 = image.Width * 3;
            float sinAngle = MathF.Sin(Angle * MathF.PI / 180);
            float cosAngle = MathF.Cos(Angle * MathF.PI / 180);

            fixed (byte* newStart = image, oldStart = original)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < width_3; x += 3)
                    {
                        float bSum = 0, gSum = 0, rSum = 0;
                        int n = 0;
                        // Iterate through each pixel of the line
                        for (int k = -length; k <= length; ++k)
                        {
                            int dx = (int)MathF.Round(k * cosAngle);
                            int dy = (int)MathF.Round(k * sinAngle);
                            int x1 = x / 3 + dx;
                            int y1 = y + dy;
                            // Skip pixels outside the image
                            if (x1 >= 0 && x1 < image.Width && y1 >= 0 && y1 < image.Height)
                            {
                                int oldIdx = y1 * width_3 + x1 * 3;
                                rSum += oldStart[oldIdx];
                                gSum += oldStart[oldIdx + 1];
                                bSum += oldStart[oldIdx + 2];
                                ++n;
                            }
                        }
                        int newIdx = y * width_3 + x;
                        newStart[newIdx] = (byte)(rSum / n);
                        newStart[newIdx + 1] = (byte)(gSum / n);
                        newStart[newIdx + 2] = (byte)(bSum / n);
                    }
                    reporter?.Report(y + 1, 0, image.Height);
                }
            }

            reporter?.Done();
        }
    }
}
