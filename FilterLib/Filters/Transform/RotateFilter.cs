using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;
using MathF = System.MathF;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate filter.
    /// </summary>
    [Filter]
    public sealed class RotateFilter : FilterBase
    {
        /// <summary>
        /// Rotation angle [0;360].
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        [FilterParamMaxF(360)]
        public float Angle { get; set; }

        /// <summary>
        /// Crop the image to fit.
        /// </summary>
        [FilterParam]
        public bool Crop { get; set; }

        /// <summary>
        /// Interpolation mode.
        /// </summary>
        [FilterParam]
        public InterpolationMode Interpolation { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="angle">Rotation angle [0;360]</param>
        /// <param name="crop">Crop the image to fit</param>
        /// <param name="interpolation">Interpolation mode</param>
        public RotateFilter(float angle = 0, bool crop = false, InterpolationMode interpolation = InterpolationMode.NearestNeighbor)
        {
            Angle = angle;
            Crop = crop;
            Interpolation = interpolation;
        }

        /// <inheritdoc/>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            if (MathF.Abs(Angle) < 0.001f) return (Bitmap)image.Clone();

            reporter?.Start();
            float angRad = Angle / 180 * MathF.PI;
            float sinAng = MathF.Sin(angRad);
            float cosAng = MathF.Cos(angRad);
            Bitmap rotated;
            if (Crop)
            {
                int newHeight = (int)(image.Height / (image.Width / (float)image.Height * Math.Abs(sinAng) + Math.Abs(cosAng)));
                rotated = new Bitmap((int)(image.Width / (float)image.Height * newHeight), newHeight);
            }
            else
            {
                rotated = new Bitmap(
                    (int)(Math.Abs(sinAng) * image.Height + Math.Abs(cosAng * image.Width)),
                    (int)(Math.Abs(cosAng) * image.Height + Math.Abs(sinAng * image.Width)));
            }

            float crx = rotated.Width / 2;
            float cry = rotated.Height / 2;
            float cx = image.Width / 2;
            float cy = image.Height / 2;
            int rotWidth_3 = rotated.Width * 3;
            int x0, y0, x1, y1;
            byte red, green, blue;

            using (DisposableBitmapData bmdRot = new(rotated, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                unsafe
                {
                    for (int y = 0; y < bmdRot.Height; ++y)
                    {
                        float yr = cry - y;
                        byte* row = (byte*)bmdRot.Scan0 + y * bmdRot.Stride;
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
                                        byte* pxOrig = (byte*)bmd.Scan0 + y0 * bmd.Stride + (x0 * 3);
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
                                        byte* pxOrig = (byte*)bmd.Scan0 + y0 * bmd.Stride + (x0 * 3);
                                        blueF += xRatio0 * yRatio0 * pxOrig[0];
                                        greenF += xRatio0 * yRatio0 * pxOrig[1];
                                        redF += xRatio0 * yRatio0 * pxOrig[2];
                                    }
                                    if (x1 >= 0 && x1 < image.Width && y0 >= 0 && y0 < image.Height)
                                    {
                                        byte* pxOrig = (byte*)bmd.Scan0 + y0 * bmd.Stride + (x1 * 3);
                                        blueF += xRatio1 * yRatio0 * pxOrig[0];
                                        greenF += xRatio1 * yRatio0 * pxOrig[1];
                                        redF += xRatio1 * yRatio0 * pxOrig[2];
                                    }
                                    if (x0 >= 0 && x0 < image.Width && y1 >= 0 && y1 < image.Height)
                                    {
                                        byte* pxOrig = (byte*)bmd.Scan0 + y1 * bmd.Stride + (x0 * 3);
                                        blueF += xRatio0 * yRatio1 * pxOrig[0];
                                        greenF += xRatio0 * yRatio1 * pxOrig[1];
                                        redF += xRatio0 * yRatio1 * pxOrig[2];
                                    }
                                    if (x1 >= 0 && x1 < image.Width && y1 >= 0 && y1 < image.Height)
                                    {
                                        byte* pxOrig = (byte*)bmd.Scan0 + y1 * bmd.Stride + (x1 * 3);
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
