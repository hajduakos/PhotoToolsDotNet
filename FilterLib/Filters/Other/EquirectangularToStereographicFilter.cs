using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterLib.Filters.Other
{
    /// <summary>
    /// Equirectangular to stereographic filter.
    /// </summary>
    [Filter]
    public sealed class EquirectangularToStereographicFilter : IFilter
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
            this.AOV = aov;
            this.Spin = spin;
        }

        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        public Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            int size = Math.Min(image.Width, image.Height); // Size of the result
            Bitmap ret = new(size, size);
            float radius = size / 4f / MathF.Tan(aov * MathF.PI / 360f); // Radius of the projection sphere
            float radiusMult2 = radius * 2;
            int sizeDiv2 = size / 2;
            int sizeMult3 = size * 3;

            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdRet = new(ret, PixelFormat.Format24bppRgb))
            {

                int w = image.Width, h = image.Height;
                int bmdStride = bmd.Stride;
                int bmdRetStride = bmdRet.Stride;
                int x, y;
                float xCorr, yCorr; // Corrected coordinates
                float lat, lng; // Latitude and longitude for a given (x,y) point
                float xOrg, yOrg; // Coordinates in the original image (float)
                float xFrac, yFrac; // Fractional parts of the coordinates
                int x0, y0, x1, y1; // Indexes in the original image (bilinear interpolation with 4 points)
                float xMultiplier = 1 / 360f * (w - 1);
                float yMultiplier = 1 / 180f * (h - 1);
                unsafe
                {
                    byte* bmdStart = (byte*)bmd.Scan0;
                    // Iterate through rows
                    for (y = 0; y < size; ++y)
                    {
                        // Get rows
                        byte* row = (byte*)bmdRet.Scan0 + (y * bmdRetStride);
                        // Iterate through columns
                        for (x = 0; x < sizeMult3; x += 3)
                        {
                            // Get corrected coordinates
                            xCorr = x / 3 - sizeDiv2;
                            yCorr = sizeDiv2 - y;

                            // Get latitude and longitude on the sphere
                            lat = 180 - 2 * MathF.Atan2(MathF.Sqrt(xCorr * xCorr + yCorr * yCorr), radiusMult2) * 180 / MathF.PI;
                            lng = MathF.Atan2(yCorr, xCorr) * 180 / MathF.PI + 180 + spin;
                            if (lng > 360) lng -= 360;

                            // Get X coordinate and points for interpolation in the original image
                            xOrg = lng * xMultiplier; // Float point
                            x0 = (int)MathF.Floor(xOrg); // First point
                            if (x0 >= w) x0 = w - 1;
                            xFrac = xOrg - x0; // Fraction part
                            x0 *= 3;
                            x1 = (int)MathF.Ceiling(xOrg);
                            if (x1 >= w) x1 = w - 1;
                            x1 *= 3; // Second point

                            // Get Y coordinate and points for interpolation in the original image
                            yOrg = lat * yMultiplier; // Float point
                            y0 = (int)MathF.Floor(yOrg); // First point
                            if (y0 >= h) y0 = h - 1;
                            yFrac = yOrg - y0; // Fraction part
                            y1 = (int)MathF.Ceiling(yOrg); // Second point
                            if (y1 >= h) y1 = h - 1;

                            // Interpolation for R,G,B components
                            row[x] = (byte)(bmdStart[y0 * bmdStride + x0] * (1 - xFrac) * (1 - yFrac)
                                + bmdStart[y1 * bmdStride + x0] * (1 - xFrac) * yFrac
                                + bmdStart[y0 * bmdStride + x1] * xFrac * (1 - yFrac)
                                + bmdStart[y1 * bmdStride + x1] * xFrac * yFrac);
                            row[x + 1] = (byte)(bmdStart[y0 * bmdStride + x0 + 1] * (1 - xFrac) * (1 - yFrac)
                                + bmdStart[y1 * bmdStride + x0 + 1] * (1 - xFrac) * yFrac
                                + bmdStart[y0 * bmdStride + x1 + 1] * xFrac * (1 - yFrac)
                                + bmdStart[y1 * bmdStride + x1 + 1] * xFrac * yFrac);
                            row[x + 2] = (byte)(bmdStart[y0 * bmdStride + x0 + 2] * (1 - xFrac) * (1 - yFrac)
                                + bmdStart[y1 * bmdStride + x0 + 2] * (1 - xFrac) * yFrac
                                + bmdStart[y0 * bmdStride + x1 + 2] * xFrac * (1 - yFrac)
                                + bmdStart[y1 * bmdStride + x1 + 2] * xFrac * yFrac);
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
                    }
                }
            }
            reporter?.Done();

            return ret;
        }
    }
}
