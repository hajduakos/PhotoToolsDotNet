using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Skew filter.
    /// </summary>
    [Filter]
    public sealed class SkewFilter : FilterBase
    {
        /// <summary>
        /// Skew angle in degrees ]-90;90[.
        /// </summary>
        [FilterParam]
        [FilterParamMinF(-90)]
        [FilterParamMaxF(90)]
        public float Angle
        {
            get;
            set { field = value.Clamp(-89.99f, 89.99f); }
        }

        /// <summary>
        /// Direction.
        /// </summary>
        [FilterParam]
        public Direction Direction { get; set; }

        /// <summary>
        /// Interpolation mode.
        /// </summary>
        [FilterParam]
        public InterpolationMode Interpolation { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="angle">Skew angle in degrees ]-90;90[</param>
        /// <param name="interpolation">Interpolation mode</param>
        public SkewFilter(float angle = 0, Direction direction = Direction.Horizontal, InterpolationMode interpolation = InterpolationMode.NearestNeighbor)
        {
            Angle = angle;
            Direction = direction;
            Interpolation = interpolation;
        }

        public override unsafe Image Apply(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            float angleRad = Angle * MathF.PI / 180f;
            float angleTan = MathF.Tan(angleRad);
            Image result = Direction switch
            {
                Direction.Horizontal => new(image.Width + (int)Math.Round(image.Height * MathF.Abs(angleTan)), image.Height),
                Direction.Vertical => new(image.Width, image.Height + (int)Math.Round(image.Width * MathF.Abs(angleTan))),
                _ => throw new ArgumentException($"Unknown skew direction: {Direction}.")
            };

            fixed (byte* oldStart = image, newStart = result)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                if (Direction == Direction.Horizontal)
                {
                    int newWidth_3 = result.Width * 3;
                    int oldWidth_3 = image.Width * 3;
                    // Iterate through rows
                    Debug.Assert(image.Height == result.Height);
                    float offset = Angle < 0 ? image.Height * MathF.Abs(angleTan) : 0;
                    for (int y = 0; y < result.Height; ++y)
                    {
                        // Skew each column
                        float dx = y * angleTan;
                        Parallel.For(0, newWidth_3, x =>
                        {
                            int x_div_3 = x / 3;
                            float old_x = x_div_3 - dx - offset;
                            int x0, x1;
                            switch (Interpolation)
                            {
                                case InterpolationMode.NearestNeighbor:
                                    x0 = (int)MathF.Round(old_x);
                                    if (0 <= x0 && x0 < image.Width) newStart0[y * newWidth_3 + x] = oldStart0[y * oldWidth_3 + x0 * 3 + x % 3];
                                    break;
                                case InterpolationMode.Bilinear:
                                    x0 = (int)MathF.Floor(old_x);
                                    x1 = (int)MathF.Ceiling(old_x);
                                    float xRatio1 = old_x - x0;
                                    float xRatio0 = 1 - xRatio1;
                                    float val = 0;
                                    if (0 <= x0 && x0 < image.Width) val += xRatio0 * oldStart0[y * oldWidth_3 + x0 * 3 + x % 3];
                                    if (0 <= x1 && x1 < image.Width) val += xRatio1 * oldStart0[y * oldWidth_3 + x1 * 3 + x % 3];
                                    newStart0[y * newWidth_3 + x] = val.ClampToByte();
                                    break;
                                default:
                                    throw new ArgumentException($"Unknown interpolation mode: {Interpolation}");
                            }
                        });
                        reporter?.Report(y + 1, 0, result.Height);
                    }
                }
                else if (Direction == Direction.Vertical)
                {
                    int width_3 = image.Width * 3;
                    // Iterate through columns
                    Debug.Assert(image.Width == result.Width);
                    float offset = Angle < 0 ? image.Width * MathF.Abs(angleTan) : 0;
                    for (int x = 0; x < width_3; ++x)
                    {
                        // Skew each row
                        int x_div_3 = x / 3;
                        float dy = x_div_3 * angleTan;
                        Parallel.For(0, result.Height, y =>
                        {
                            float old_y = y - dy - offset;
                            int y0, y1;
                            switch (Interpolation)
                            {
                                case InterpolationMode.NearestNeighbor:
                                    y0 = (int)MathF.Round(old_y);
                                    if (0 <= y0 && y0 < image.Height) newStart0[y * width_3 + x] = oldStart0[y0 * width_3 + x];
                                    break;
                                case InterpolationMode.Bilinear:
                                    y0 = (int)MathF.Floor(old_y);
                                    y1 = (int)MathF.Ceiling(old_y);
                                    float yRatio1 = old_y - y0;
                                    float yRatio0 = 1 - yRatio1;
                                    float val = 0;
                                    if (0 <= y0 && y0 < image.Height) val += yRatio0 * oldStart0[y0 * width_3 + x];
                                    if (0 <= y1 && y1 < image.Height) val += yRatio1 * oldStart0[y1 * width_3 + x];
                                    newStart0[y * width_3 + x] = val.ClampToByte();
                                    break;
                            }
                        });
                        reporter?.Report(x + 1, 0, width_3);
                    }
                }
                else throw new ArgumentException($"Unknown skew direction: {Direction}.");
            }

            reporter?.Done();
            return result;
        }
    }
}
