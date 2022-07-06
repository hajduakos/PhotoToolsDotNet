using FilterLib.Reporting;
using MathF = System.MathF;

namespace FilterLib.Filters.Other
{
    /// <summary>
    /// Convert to polar filter.
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
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();

            unsafe
            {
                fixed (byte* start = image, origStart = original)
                {
                    int w = image.Width, h = image.Height;
                    int width_3 = image.Width * 3; // Width of a row
                    int x, y;
                    float xCorr, yCorr; // Corrected coordinates
                    float halfHeight = (h - 1) / 2f;
                    float halfWidth = (w - 1) / 2f;
                    float r, fi; // Radius and angle
                    float xMult = h / (float)w; // Correction to circle
                    int x0, y0, x1, y1; // Indexes in the original image (bilinear interpolation with 4 points)
                    float xOrg, yOrg; // Coordinates in the original image (float)
                    float xFrac, yFrac; // Fractional parts of the coordinates
                    float xMultiplier = (1 / (2 * MathF.PI) * (w - 1)); // Multiplier to convert to polar
                    float yMultiplier = 1 / halfHeight * (h - 1); // Multiplier to convert to polar

                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get rows
                        byte* row = start + (y * width_3);
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            // Get corrected coordinates
                            xCorr = ((x / 3) - halfWidth) * xMult;
                            yCorr = halfHeight - y;
                            // Get radius and angle
                            r = MathF.Sqrt(xCorr * xCorr + yCorr * yCorr);
                            fi = (-MathF.Atan2(xCorr, yCorr) + MathF.PI + phase / 180f * MathF.PI);

                            if (fi >= 2 * MathF.PI) fi -= 2 * MathF.PI;

                            // Get X coordinate and points for interpolation in the original image
                            xOrg = fi * xMultiplier;

                            x0 = (int)MathF.Floor(xOrg);
                            xFrac = xOrg - x0;
                            if (x0 >= w) x0 = w - 1;
                            x0 *= 3;

                            x1 = (int)MathF.Ceiling(xOrg);
                            if (x1 >= w) x1 = w - 1;
                            x1 *= 3;

                            // Get Y coordinate and points for interpolation in the original image
                            yOrg = r * yMultiplier;

                            y0 = (int)MathF.Floor(yOrg);
                            yFrac = yOrg - y0;

                            if (y0 >= h) y0 = h - 1;

                            y1 = (int)MathF.Ceiling(yOrg);
                            if (y1 >= h) y1 = h - 1;

                            // Interpolation for R,G,B components
                            row[x] = (byte)(origStart[y0 * width_3 + x0] * (1 - xFrac) * (1 - yFrac)
                                + origStart[y1 * width_3 + x0] * (1 - xFrac) * yFrac
                                + origStart[y0 * width_3 + x1] * xFrac * (1 - yFrac)
                                + origStart[y1 * width_3 + x1] * xFrac * yFrac);
                            row[x + 1] = (byte)(origStart[y0 * width_3 + x0 + 1] * (1 - xFrac) * (1 - yFrac)
                                + origStart[y1 * width_3 + x0 + 1] * (1 - xFrac) * yFrac
                                + origStart[y0 * width_3 + x1 + 1] * xFrac * (1 - yFrac)
                                + origStart[y1 * width_3 + x1 + 1] * xFrac * yFrac);
                            row[x + 2] = (byte)(origStart[y0 * width_3 + x0 + 2] * (1 - xFrac) * (1 - yFrac)
                                + origStart[y1 * width_3 + x0 + 2] * (1 - xFrac) * yFrac
                                + origStart[y0 * width_3 + x1 + 2] * xFrac * (1 - yFrac)
                                + origStart[y1 * width_3 + x1 + 2] * xFrac * yFrac);
                        }
                        reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
