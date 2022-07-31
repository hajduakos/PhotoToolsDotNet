using FilterLib.Reporting;
using FilterLib.Util;
using System;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Create a blur effect as if the camera was spinning: for each pixel, sample pixels along
    /// an arc around the center point of the blur.
    /// </summary>
    [Filter]
    public sealed class SpinBlurFilter : FilterInPlaceBase
    {
        /// <summary>
        /// X coordinate of the center.
        /// </summary>
        [FilterParam]
        public Size CenterX { get; set; }

        /// <summary>
        /// Y coordinate of the center.
        /// </summary>
        [FilterParam]
        public Size CenterY { get; set; }

        private float angle;
        private int samples;

        /// <summary>
        /// Blur angle [0;360].
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        [FilterParamMaxF(360)]
        public float Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                while (angle > 360) angle -= 360;
                while (angle < 0) angle += 360;
            }
        }

        /// <summary>
        /// Number of samples [2;...]
        /// </summary>
        [FilterParam]
        [FilterParamMin(2)]
        public int Samples
        {
            get { return samples; }
            set { samples = Math.Max(2, value); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="centerX">X coordinate of the center</param>
        /// <param name="centerY">Y coordinate of the center</param>
        /// <param name="angle">Blur angle [0;360]</param>
        /// <param name="samples">Number of samples [2;...]</param>
        public SpinBlurFilter(Size centerX, Size centerY, float angle, int samples)
        {
            CenterX = centerX;
            CenterY = centerY;
            Angle = angle;
            Samples = samples;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SpinBlurFilter() : this(Size.Absolute(0), Size.Absolute(0), 0, 2) { }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(image.Width == original.Width);
            float cx = CenterX.ToAbsolute(image.Width);
            float cy = CenterY.ToAbsolute(image.Height);
            int width_3 = image.Width * 3;
            float da = angle / (samples - 1);
            fixed (byte* newStart = image, oldStart = original)
            {
                byte* newPx = newStart;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        // Do the sampling going along the arc, half on one
                        // side of the pixel, and half on the other side
                        float r = 0, g = 0, b = 0;
                        int n = 0;
                        for (int i = 0; i < samples; ++i)
                        {
                            float a = (-angle / 2f + da * i) * MathF.PI / 180;
                            int x0 = (int)MathF.Round(MathF.Cos(a) * (x - cx) - MathF.Sin(a) * (y - cy) + cx);
                            int y0 = (int)MathF.Round(MathF.Sin(a) * (x - cx) + MathF.Cos(a) * (y - cy) + cy);
                            if (0 <= x0 && x0 < image.Width && 0 <= y0 && y0 < image.Height)
                            {
                                byte* oldPx = oldStart + y0 * width_3 + x0 * 3;
                                r += oldPx[0];
                                g += oldPx[1];
                                b += oldPx[2];
                                ++n;
                            }
                        }
                        // We might get no samples (e.g. corners): just use the old pixel then
                        if (n == 0)
                        {
                            byte* oldPx = oldStart + y * width_3 + x * 3;
                            r += oldPx[0];
                            g += oldPx[1];
                            b += oldPx[2];
                            ++n;
                        }

                        newPx[0] = (r / n).ClampToByte();
                        newPx[1] = (g / n).ClampToByte();
                        newPx[2] = (b / n).ClampToByte();
                        newPx += 3;
                    }
                    reporter?.Report(y + 1, 0, image.Height);
                }
            }
            reporter?.Done();
        }
    }
}
