using FilterLib.Reporting;
using FilterLib.Util;
using System;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Create a blur effect as if the camera was zooming: for each pixel, sample pixels along
    /// the line connecting the pixel to the center point of the blur.
    /// </summary>
    [Filter]
    public sealed class ZoomBlurFilter : FilterInPlaceBase
    {
        private int amount;
        private int maxSamples;

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

        /// <summary>
        /// Blur amount [0;100].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(100)]
        public int Amount
        {
            get { return amount; }
            set { amount = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Maximal number of samples [2;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(2)]
        public int MaxSamples
        {
            get { return maxSamples; }
            set { maxSamples = Math.Max(2, value); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="centerX">X coordinate of the center</param>
        /// <param name="centerY">Y coordinate of the center</param>
        /// <param name="amount">Blur amount [0;100]</param>
        /// <param name="maxSamples">Maximal number of samples [2;...]</param>
        public ZoomBlurFilter(Size centerX, Size centerY, int amount, int maxSamples)
        {
            CenterX = centerX;
            CenterY = centerY;
            Amount = amount;
            MaxSamples = maxSamples;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ZoomBlurFilter() : this(Size.Absolute(0), Size.Absolute(0), 0, 2) { }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(image.Width == original.Width);
            float cx = CenterX.ToAbsolute(image.Width);
            float cy = CenterY.ToAbsolute(image.Height);
            float amountF = amount / 100f;
            int width_3 = image.Width * 3;
            fixed (byte* newStart = image, oldStart = original)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                Parallel.For(0, image.Height, y =>
                {
                    byte* newPx = newStart0 + y * width_3;
                    for (int x = 0; x < image.Width; ++x)
                    {
                        float distanceFromCenter = MathF.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
                        float len = distanceFromCenter * amountF;
                        // Number of samples are based on the length (but at least two)
                        int samples = Math.Max(2, Math.Min(maxSamples, (int)Math.Ceiling(len)));
                        float xLen = (cx - x) * amountF;
                        float yLen = (cy - y) * amountF;
                        float dx = xLen / (samples - 1);
                        float dy = yLen / (samples - 1);
                        // Do the sampling going along the line, half on one
                        // side of the pixel, and half on the other side
                        float r = 0, g = 0, b = 0;
                        int n = 0;
                        for (int i = 0; i < samples; ++i)
                        {
                            int x0 = (int)Math.Round(x + i * dx - xLen / 2);
                            int y0 = (int)Math.Round(y + i * dy - yLen / 2);
                            if (0 <= x0 && x0 < image.Width && 0 <= y0 && y0 < image.Height)
                            {
                                byte* oldPx = oldStart0 + y0 * width_3 + x0 * 3;
                                r += oldPx[0];
                                g += oldPx[1];
                                b += oldPx[2];
                                ++n;
                            }
                        }
                        // If the center point is somewhere outside in a weird position,
                        // we might get no samples: just use the old pixel then
                        if (n == 0)
                        {
                            byte* oldPx = oldStart0 + y * width_3 + x * 3;
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
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }
            reporter?.Done();
        }
    }
}
