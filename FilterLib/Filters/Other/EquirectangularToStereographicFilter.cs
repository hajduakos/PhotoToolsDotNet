using FilterLib.Reporting;
using FilterLib.Util;
using Math = System.Math;
using MathF = System.MathF;

namespace FilterLib.Filters.Other
{
    /// <summary>
    /// Assume the input to have an equirectangular projection and re-project as stereoraphic.
    /// </summary>
    [Filter]
    public sealed class EquirectangularToStereographicFilter : FilterBase
    {
        private float aov;
        private float spin;

        /// <summary>
        /// Angle of view [0;180[
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        [FilterParamMaxF(179.999f)]
        public float AOV
        {
            get { return aov; }
            set { aov = value.Clamp(0, 179.999f); }
        }

        /// <summary>
        /// Spin [0;360]
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        [FilterParamMaxF(360)]
        public float Spin
        {
            get { return spin; }
            set
            {
                spin = value;
                while (spin > 360) spin -= 360;
                while (spin < 0) spin += 360;
            }
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        /// <param name="aov">Angle of view [0;180[</param>
        /// <param name="spin">Spin [0;360]</param>
        public EquirectangularToStereographicFilter(float aov = 120, float spin = 0)
        {
            AOV = aov;
            Spin = spin;
        }

        /// <inheritdoc/>
        public override unsafe Image Apply(Image image, IReporter reporter = null)
        {
            // We assume that the input image represents an equirectangular projection of a sphere, where the
            // X coordinate corresponds to longitudes (0 to 360°) and the y coordinate corresponds to latitudes
            // (0 to 180°). Then, we reproject this sphere using stereographic projection: we put the sphere
            // on a plane and project lines from the North pole towards the plane. The lines hit both the
            // sphere and the plane. The color on the plane (the result) will take the color from the sphere
            // intersection.
            reporter?.Start();
            int newSize = Math.Min(image.Width, image.Height);
            Image result = new(newSize, newSize);
            float radius = newSize / 4f / MathF.Tan(aov * MathF.PI / 360f); // Radius of the projection sphere
            float radiusMult2 = radius * 2;
            int newSize_div2 = newSize / 2;
            int newSize_3 = newSize * 3;
            int oldWidth_3 = image.Width * 3;
            float xMult = 1 / 360f * (image.Width - 1);
            float yMult = 1 / 180f * (image.Height - 1);
            fixed (byte* oldStart = image, newStart = result)
            {
                byte* newPtr = newStart;
                for (int y = 0; y < newSize; ++y)
                {
                    for (int x = 0; x < newSize_3; x += 3)
                    {
                        // Get corrected coordinates
                        float xCorr = x / 3 - newSize_div2;
                        float yCorr = newSize_div2 - y;

                        // Get latitude and longitude on the sphere
                        float lat = 180 - 2 * MathF.Atan2(MathF.Sqrt(xCorr * xCorr + yCorr * yCorr), radiusMult2) * 180 / MathF.PI;
                        float lng = MathF.Atan2(yCorr, xCorr) * 180 / MathF.PI + 180 + spin;
                        if (lng > 360) lng -= 360;

                        // Get X coordinate and points for interpolation in the original image
                        float xOrg = lng * xMult; // Float point
                        int x0 = (int)MathF.Floor(xOrg); // First point
                        if (x0 >= image.Width) x0 = image.Width - 1;
                        float xFrac = xOrg - x0; // Fraction part
                        x0 *= 3;
                        int x1 = (int)MathF.Ceiling(xOrg);
                        if (x1 >= image.Width) x1 = image.Width - 1;
                        x1 *= 3; // Second point

                        // Get Y coordinate and points for interpolation in the original image
                        float yOrg = lat * yMult; // Float point
                        int y0 = (int)MathF.Floor(yOrg); // First point
                        if (y0 >= image.Height) y0 = image.Height - 1;
                        float yFrac = yOrg - y0; // Fraction part
                        int y1 = (int)MathF.Ceiling(yOrg); // Second point
                        if (y1 >= image.Height) y1 = image.Height - 1;

                        // Bilinear interpolation
                        newPtr[0] = (byte)(oldStart[y0 * oldWidth_3 + x0] * (1 - xFrac) * (1 - yFrac)
                            + oldStart[y1 * oldWidth_3 + x0] * (1 - xFrac) * yFrac
                            + oldStart[y0 * oldWidth_3 + x1] * xFrac * (1 - yFrac)
                            + oldStart[y1 * oldWidth_3 + x1] * xFrac * yFrac);
                        newPtr[1] = (byte)(oldStart[y0 * oldWidth_3 + x0 + 1] * (1 - xFrac) * (1 - yFrac)
                            + oldStart[y1 * oldWidth_3 + x0 + 1] * (1 - xFrac) * yFrac
                            + oldStart[y0 * oldWidth_3 + x1 + 1] * xFrac * (1 - yFrac)
                            + oldStart[y1 * oldWidth_3 + x1 + 1] * xFrac * yFrac);
                        newPtr[2] = (byte)(oldStart[y0 * oldWidth_3 + x0 + 2] * (1 - xFrac) * (1 - yFrac)
                            + oldStart[y1 * oldWidth_3 + x0 + 2] * (1 - xFrac) * yFrac
                            + oldStart[y0 * oldWidth_3 + x1 + 2] * xFrac * (1 - yFrac)
                            + oldStart[y1 * oldWidth_3 + x1 + 2] * xFrac * yFrac);
                        newPtr += 3;
                    }
                    reporter?.Report(y, 0, image.Height - 1);
                }
            }

            reporter?.Done();
            return result;
        }
    }
}
