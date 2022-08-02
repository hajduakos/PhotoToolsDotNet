using FilterLib.Reporting;
using MathF = System.MathF;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Other
{
    /// <summary>
    /// Convert cartesian coordinates to polar to get a circular effect.
    /// </summary>
    [Filter]
    public sealed class ConvertToPolarFilter : FilterInPlaceBase
    {
        private float phase;

        /// <summary>
        /// Phase [0;360].
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        [FilterParamMaxF(360)]
        public float Phase
        {
            get { return phase; }
            set
            {
                phase = value;
                while (phase > 360) phase -= 360;
                while (phase < 0) phase += 360;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="phase">Phase [0;360]</param>
        public ConvertToPolarFilter(float phase = 0) => Phase = phase;

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();

            int width_3 = image.Width * 3;
            float halfHeight = (image.Height - 1) / 2f;
            float halfWidth = (image.Width - 1) / 2f;
            float aspect = image.Height / (float)image.Width; // Correction to circle
            float xMul = 1 / (2 * MathF.PI) * (image.Width - 1); // Multiplier to convert to polar
            float yMul = 1 / halfHeight * (image.Height - 1); // Multiplier to convert to polar

            fixed (byte* newStart = image, oldStart = original)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                Parallel.For(0, image.Height, y =>
                {
                    byte* newRow = newStart0 + y * width_3;
                    for (int x = 0; x < width_3; x += 3)
                    {
                        // Get corrected coordinates
                        float xCorr = ((x / 3) - halfWidth) * aspect;
                        float yCorr = halfHeight - y;
                        // Get radius and angle
                        float r = MathF.Sqrt(xCorr * xCorr + yCorr * yCorr);
                        float fi = (-MathF.Atan2(xCorr, yCorr) + MathF.PI + phase / 180f * MathF.PI);

                        if (fi >= 2 * MathF.PI) fi -= 2 * MathF.PI;

                        // Get X coordinate and points for interpolation in the original image
                        float xOrg = fi * xMul;

                        int x0 = (int)MathF.Floor(xOrg);
                        float xFrac = xOrg - x0;
                        if (x0 >= image.Width) x0 = image.Width - 1;
                        x0 *= 3;

                        int x1 = (int)MathF.Ceiling(xOrg);
                        if (x1 >= image.Width) x1 = image.Width - 1;
                        x1 *= 3;

                        // Get Y coordinate and points for interpolation in the original image
                        float yOrg = r * yMul;

                        int y0 = (int)MathF.Floor(yOrg);
                        float yFrac = yOrg - y0;

                        if (y0 >= image.Height) y0 = image.Height - 1;

                        int y1 = (int)MathF.Ceiling(yOrg);
                        if (y1 >= image.Height) y1 = image.Height - 1;

                        // Bilinear interpolation
                        newRow[x] = (byte)(oldStart0[y0 * width_3 + x0] * (1 - xFrac) * (1 - yFrac)
                            + oldStart0[y1 * width_3 + x0] * (1 - xFrac) * yFrac
                            + oldStart0[y0 * width_3 + x1] * xFrac * (1 - yFrac)
                            + oldStart0[y1 * width_3 + x1] * xFrac * yFrac);
                        newRow[x + 1] = (byte)(oldStart0[y0 * width_3 + x0 + 1] * (1 - xFrac) * (1 - yFrac)
                            + oldStart0[y1 * width_3 + x0 + 1] * (1 - xFrac) * yFrac
                            + oldStart0[y0 * width_3 + x1 + 1] * xFrac * (1 - yFrac)
                            + oldStart0[y1 * width_3 + x1 + 1] * xFrac * yFrac);
                        newRow[x + 2] = (byte)(oldStart0[y0 * width_3 + x0 + 2] * (1 - xFrac) * (1 - yFrac)
                            + oldStart0[y1 * width_3 + x0 + 2] * (1 - xFrac) * yFrac
                            + oldStart0[y0 * width_3 + x1 + 2] * xFrac * (1 - yFrac)
                            + oldStart0[y1 * width_3 + x1 + 2] * xFrac * yFrac);
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }
            reporter?.Done();
        }
    }
}
