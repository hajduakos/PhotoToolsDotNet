using FilterLib.Filters.Transform;
using FilterLib.Reporting;
using FilterLib.Util;
using Math = System.Math;
using MathF = System.MathF;
using Parallel = System.Threading.Tasks.Parallel;

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
        /// Interpolation mode.
        /// </summary>
        [FilterParam]
        public InterpolationMode Interpolation { get; set; }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        /// <param name="aov">Angle of view [0;180[</param>
        /// <param name="spin">Spin [0;360]</param>
        public EquirectangularToStereographicFilter(float aov = 120, float spin = 0, InterpolationMode interpolation = InterpolationMode.NearestNeighbor)
        {
            AOV = aov;
            Spin = spin;
            Interpolation = interpolation;
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
            object reporterLock = new();
            int progress = 0;
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
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                Parallel.For(0, newSize, y =>
                {
                    byte* newPtr = newStart0 + y * newSize_3;
                    for (int x = 0; x < newSize_3; x += 3)
                    {
                        // Get corrected coordinates
                        float xCorr = x / 3 - newSize_div2;
                        float yCorr = newSize_div2 - y;

                        // Get latitude and longitude on the sphere
                        float lat = 180 - 2 * MathF.Atan2(MathF.Sqrt(xCorr * xCorr + yCorr * yCorr), radiusMult2) * 180 / MathF.PI;
                        float lng = MathF.Atan2(yCorr, xCorr) * 180 / MathF.PI + 180 + spin;
                        if (lng > 360) lng -= 360;

                        float xOrg = lng * xMult;
                        float yOrg = lat * yMult;
                        int x0, y0, x1, y1;
                        switch (Interpolation)
                        {
                            case InterpolationMode.NearestNeighbor:
                                x0 = Math.Min((int)MathF.Round(xOrg), image.Width - 1);
                                y0 = Math.Min((int)MathF.Round(yOrg), image.Height - 1);
                                byte* oldPx = oldStart0 + y0 * oldWidth_3 + x0 * 3;
                                for (int i = 0; i < 3; ++i) newPtr[i] = oldPx[i];
                                break;
                            case InterpolationMode.Bilinear:
                                x0 = Math.Min((int)MathF.Floor(xOrg), image.Width - 1);
                                x1 = Math.Min((int)MathF.Ceiling(xOrg), image.Width - 1);
                                float xRatio1 = xOrg - x0;
                                float xRatio0 = 1 - xRatio1;
                                x0 *= 3;
                                x1 *= 3;
                                y0 = Math.Min((int)MathF.Floor(yOrg), image.Height - 1);
                                y1 = Math.Min((int)MathF.Ceiling(yOrg), image.Height - 1);
                                float yRatio1 = yOrg - y0;
                                float yRatio0 = 1 - yRatio1;
                                byte* oldRow0 = oldStart0 + y0 * oldWidth_3;
                                byte* oldRow1 = oldStart0 + y1 * oldWidth_3;
                                for (int i = 0; i < 3; ++i)
                                    newPtr[i] = (byte)(
                                        oldRow0[x0 + i] * xRatio0 * yRatio0 +
                                        oldRow1[x0 + i] * xRatio0 * yRatio1 +
                                        oldRow0[x1 + i] * xRatio1 * yRatio0 +
                                        oldRow1[x1 + i] * xRatio1 * yRatio1);
                                break;
                            default:
                                throw new System.ArgumentException($"Unknown interpolation mode: {Interpolation}.");
                        }
                        newPtr += 3;
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }

            reporter?.Done();
            return result;
        }
    }
}
