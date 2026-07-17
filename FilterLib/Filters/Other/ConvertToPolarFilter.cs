using FilterLib.Reporting;
using FilterLib.Util;
using MathF = System.MathF;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Other;

[Filter("Convert cartesian coordinates to polar to get a circular effect.")]
public sealed class ConvertToPolarFilter : FilterInPlaceBase
{
    /// <summary>
    /// Phase [0;360].
    /// </summary>
    [FilterParam]
    [FilterParamMinF(0)]
    [FilterParamMaxF(360)]
    public float Phase
    {
        get;
        set
        {
            field = value;
            while (field > 360) field -= 360;
            while (field < 0) field += 360;
        }
    }

    /// <summary>
    /// Interpolation mode.
    /// </summary>
    [FilterParam]
    public InterpolationMode Interpolation { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="phase">Phase [0;360]</param>
    /// <param name="interpolation">Interpolation mode</param>
    public ConvertToPolarFilter(float phase = 0, InterpolationMode interpolation = InterpolationMode.NearestNeighbor)
    {
        Phase = phase;
        Interpolation = interpolation;
    }

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
                    float fi = (-MathF.Atan2(xCorr, yCorr) + MathF.PI + Phase / 180f * MathF.PI);

                    if (fi >= 2 * MathF.PI) fi -= 2 * MathF.PI;

                    // Get coordinates in the original image
                    float xOrg = fi * xMul;
                    float yOrg = r * yMul;

                    switch (Interpolation)
                    {
                        case InterpolationMode.NearestNeighbor:
                            int xn = (int)MathF.Round(xOrg);
                            if (xn >= image.Width) xn = image.Width - 1;
                            xn *= 3;
                            int yn = (int)MathF.Round(yOrg);
                            if (yn >= image.Height) yn = image.Height - 1;
                            byte* oldRow = oldStart0 + yn * width_3;
                            newRow[x] = oldRow[xn];
                            newRow[x + 1] = oldRow[xn + 1];
                            newRow[x + 2] = oldRow[xn + 2];
                            break;
                        case InterpolationMode.Bilinear:
                            int x0 = (int)MathF.Floor(xOrg);
                            float xFrac = xOrg - x0;
                            if (x0 >= image.Width) x0 = image.Width - 1;
                            x0 *= 3;

                            int x1 = (int)MathF.Ceiling(xOrg);
                            if (x1 >= image.Width) x1 = image.Width - 1;
                            x1 *= 3;

                            int y0 = (int)MathF.Floor(yOrg);
                            float yFrac = yOrg - y0;

                            if (y0 >= image.Height) y0 = image.Height - 1;

                            int y1 = (int)MathF.Ceiling(yOrg);
                            if (y1 >= image.Height) y1 = image.Height - 1;

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
                            break;
                        default:
                            throw new System.ArgumentException($"Unknown interpolation mode: {Interpolation}.");
                    }
                }
                if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
            });
        }
        reporter?.Done();
    }
}
