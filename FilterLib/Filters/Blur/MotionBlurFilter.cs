using FilterLib.Reporting;
using Math = System.Math;
using MathF = System.MathF;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Blur
{
    [Filter("Blur by replacing each pixel with the average calculated along a line.")]
    public sealed class MotionBlurFilter : FilterInPlaceBase
    {
        /// <summary>
        /// Blur length [0;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int Length
        {
            get;
            set { field = Math.Max(0, value); }
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
            object reporterLock = new();
            int progress = 0;
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            System.Diagnostics.Debug.Assert(image.Width == original.Width);
            int width_3 = image.Width * 3;
            float sinAngle = MathF.Sin(Angle * MathF.PI / 180);
            float cosAngle = MathF.Cos(Angle * MathF.PI / 180);

            fixed (byte* newStart = image, oldStart = original)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                Parallel.For(0, image.Height, y =>
                {
                    byte* newPx = newStart0 + y * width_3;
                    for (int x = 0; x < width_3; x += 3)
                    {
                        float bSum = 0, gSum = 0, rSum = 0;
                        int n = 0;
                        // Iterate through each pixel of the line
                        for (int k = -Length; k <= Length; ++k)
                        {
                            int dx = (int)MathF.Round(k * cosAngle);
                            int dy = (int)MathF.Round(k * sinAngle);
                            int x1 = x / 3 + dx;
                            int y1 = y + dy;
                            // Skip pixels outside the image
                            if (x1 >= 0 && x1 < image.Width && y1 >= 0 && y1 < image.Height)
                            {
                                int oldIdx = y1 * width_3 + x1 * 3;
                                rSum += oldStart0[oldIdx];
                                gSum += oldStart0[oldIdx + 1];
                                bSum += oldStart0[oldIdx + 2];
                                ++n;
                            }
                        }
                        newPx[0] = (byte)(rSum / n);
                        newPx[1] = (byte)(gSum / n);
                        newPx[2] = (byte)(bSum / n);
                        newPx += 3;
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }

            reporter?.Done();
        }
    }
}
