using FilterLib.Reporting;
using FilterLib.Util;
using Math = System.Math;
using MathF = System.MathF;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate filter.
    /// </summary>
    [Filter]
    public sealed class RotateFilter : FilterBase
    {
        public enum CropMode { Fit, Fill }

        /// <summary>
        /// Rotation angle [0;360].
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        [FilterParamMaxF(360)]
        public float Angle { get; set; }

        /// <summary>
        /// Crop mode (fit or fill).
        /// </summary>
        [FilterParam]
        public CropMode Crop { get; set; }

        /// <summary>
        /// Interpolation mode.
        /// </summary>
        [FilterParam]
        public InterpolationMode Interpolation { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="angle">Rotation angle [0;360]</param>
        /// <param name="crop">Crop mode</param>
        /// <param name="interpolation">Interpolation mode</param>
        public RotateFilter(float angle = 0, CropMode crop = CropMode.Fit, InterpolationMode interpolation = InterpolationMode.NearestNeighbor)
        {
            Angle = angle;
            Crop = crop;
            Interpolation = interpolation;
        }

        /// <inheritdoc/>
        public override Image Apply(Image image, IReporter reporter = null)
        {
            if (MathF.Abs(Angle) < 0.001f) return (Image)image.Clone();

            reporter?.Start();
            float angRad = Angle / 180 * MathF.PI;
            float sinAng = MathF.Sin(angRad);
            float cosAng = MathF.Cos(angRad);
            Image rotated = null;
            switch (Crop)
            {
                case CropMode.Fill:
                    int newHeight = (int)(image.Height / (image.Width / (float)image.Height * Math.Abs(sinAng) + Math.Abs(cosAng)));
                    rotated = new Image((int)(image.Width / (float)image.Height * newHeight), newHeight);
                    break;
                case CropMode.Fit:
                    rotated = new Image(
                        (int)(Math.Abs(sinAng) * image.Height + Math.Abs(cosAng) * image.Width),
                        (int)(Math.Abs(cosAng) * image.Height + Math.Abs(sinAng) * image.Width));
                    break;
                default:
                    throw new System.ArgumentException($"Unknown crop mode: {Crop}");
            }
            System.Diagnostics.Debug.Assert(rotated != null);

            float crx = rotated.Width / 2;
            float cry = rotated.Height / 2;
            float cx = image.Width / 2;
            float cy = image.Height / 2;
            int rotWidth_3 = rotated.Width * 3;
            int x0, y0, x1, y1;
            byte red, green, blue;

            unsafe
            {
                fixed (byte* start = image, rotStart = rotated)
                {
                    for (int y = 0; y < rotated.Height; ++y)
                    {
                        float yr = cry - y;
                        byte* row = rotStart + y * rotWidth_3;
                        for (int x = 0; x < rotWidth_3; x += 3)
                        {
                            float xr = x / 3 - crx;
                            float xOrig = xr * cosAng - yr * sinAng + cx;
                            float yOrig = cy - (xr * sinAng + yr * cosAng);
                            red = green = blue = 0;
                            switch (Interpolation)
                            {
                                case InterpolationMode.NearestNeighbor:
                                    x0 = (int)Math.Round(xOrig);
                                    y0 = (int)Math.Round(yOrig);
                                    if (x0 >= 0 && x0 < image.Width && y0 >= 0 && y0 < image.Height)
                                    {
                                        byte* pxOrig = start + y0 * image.Width * 3 + (x0 * 3);
                                        blue = pxOrig[0];
                                        green = pxOrig[1];
                                        red = pxOrig[2];
                                    }
                                    break;
                                case InterpolationMode.Bilinear:
                                    x0 = (int)Math.Floor(xOrig);
                                    y0 = (int)Math.Floor(yOrig);
                                    x1 = (int)Math.Ceiling(xOrig);
                                    y1 = (int)Math.Ceiling(yOrig);
                                    float xRatio1 = xOrig - x0;
                                    float xRatio0 = 1 - xRatio1;
                                    float yRatio1 = yOrig - y0;
                                    float yRatio0 = 1 - yRatio1;
                                    float redF = 0, greenF = 0, blueF = 0;
                                    if (x0 >= 0 && x0 < image.Width && y0 >= 0 && y0 < image.Height)
                                    {
                                        byte* pxOrig = start + y0 * image.Width * 3 + (x0 * 3);
                                        blueF += xRatio0 * yRatio0 * pxOrig[0];
                                        greenF += xRatio0 * yRatio0 * pxOrig[1];
                                        redF += xRatio0 * yRatio0 * pxOrig[2];
                                    }
                                    if (x1 >= 0 && x1 < image.Width && y0 >= 0 && y0 < image.Height)
                                    {
                                        byte* pxOrig = start + y0 * image.Width * 3 + (x1 * 3);
                                        blueF += xRatio1 * yRatio0 * pxOrig[0];
                                        greenF += xRatio1 * yRatio0 * pxOrig[1];
                                        redF += xRatio1 * yRatio0 * pxOrig[2];
                                    }
                                    if (x0 >= 0 && x0 < image.Width && y1 >= 0 && y1 < image.Height)
                                    {
                                        byte* pxOrig = start + y1 * image.Width * 3 + (x0 * 3);
                                        blueF += xRatio0 * yRatio1 * pxOrig[0];
                                        greenF += xRatio0 * yRatio1 * pxOrig[1];
                                        redF += xRatio0 * yRatio1 * pxOrig[2];
                                    }
                                    if (x1 >= 0 && x1 < image.Width && y1 >= 0 && y1 < image.Height)
                                    {
                                        byte* pxOrig = start + y1 * image.Width * 3 + (x1 * 3);
                                        blueF += xRatio1 * yRatio1 * pxOrig[0];
                                        greenF += xRatio1 * yRatio1 * pxOrig[1];
                                        redF += xRatio1 * yRatio1 * pxOrig[2];
                                    }
                                    blue = blueF.ClampToByte();
                                    green = greenF.ClampToByte();
                                    red = redF.ClampToByte();
                                    break;
                                default:
                                    throw new System.ArgumentException($"Unknown interpolation mode: {Interpolation}");
                            }
                            row[x] = blue;
                            row[x + 1] = green;
                            row[x + 2] = red;
                        }
                        reporter?.Report(y, 0, image.Height - 1);
                    }
                }
            }


            reporter?.Done();
            return rotated;
        }
    }
}
