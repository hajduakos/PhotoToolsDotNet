using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Perspective transformation filter.
    /// </summary>
    [Filter]
    public class PerspectiveFilter : FilterBase
    {
        /// <summary>
        /// Scale percentage.
        /// </summary>
        [FilterParam]
        public float Scale { get; set; }

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
        /// <param name="scale">Scale percentage</param>
        /// <param name="direction">Direction</param>
        /// <param name="interpolation">Interpolation mode</param>
        public PerspectiveFilter(float scale = 1, Direction direction = Direction.Horizontal, InterpolationMode interpolation = InterpolationMode.NearestNeighbor)
        {
            Scale = scale;
            Direction = direction;
            Interpolation = interpolation;
        }

        public override unsafe Image Apply(Image image, IReporter reporter = null)
        {
            float scaleAbs = MathF.Abs(Scale);
            reporter?.Start();
            Image result;
            if (Direction == Direction.Horizontal)
                result = new(Math.Max(image.Width, (int)(image.Width * scaleAbs)), image.Height);
            else if (Direction == Direction.Vertical)
                result = new(image.Width, Math.Max(image.Height, (int)(image.Height * scaleAbs)));
            else
                throw new ArgumentException($"Unknown perspective direction: {Direction}.");

            fixed (byte* oldStart = image, newStart = result)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                if (Direction == Direction.Horizontal)
                {
                    int topLeft = 0, bottomLeft = 0;
                    int smallerSide = Math.Min(image.Width, (int)(image.Width * scaleAbs));
                    if (Scale > 0 && scaleAbs >= 1 || Scale < 0 && scaleAbs < 1)
                        topLeft = (result.Width - smallerSide) / 2;
                    else
                        bottomLeft = (result.Width - smallerSide) / 2;

                    int newWidth_3 = result.Width * 3;
                    int oldWidth_3 = image.Width * 3;
                    // Iterate through rows
                    Debug.Assert(image.Height == result.Height);
                    for (int y = 0; y < result.Height; ++y)
                    {
                        // Transform each column
                        float rowOffset = topLeft + (bottomLeft - topLeft) * (y / (result.Height - 1f));
                        float rowScale = (result.Width - 2 * rowOffset) / (float)image.Width;
                        Parallel.For(0, newWidth_3, x =>
                        {
                            int x_div_3 = x / 3;
                            float x_orig = (x_div_3 - rowOffset) / rowScale;
                            int x0, x1;
                            switch (Interpolation)
                            {
                                case InterpolationMode.NearestNeighbor:
                                    x0 = (int)MathF.Round(x_orig);
                                    if (0 <= x0 && x0 < image.Width) newStart0[y * newWidth_3 + x] = oldStart0[y * oldWidth_3 + x0 * 3 + (x % 3)];
                                    break;
                                case InterpolationMode.Bilinear:
                                    x0 = (int)MathF.Floor(x_orig);
                                    x1 = (int)MathF.Ceiling(x_orig);
                                    float xRatio1 = x_orig - x0;
                                    float xRatio0 = 1 - xRatio1;
                                    float val = 0;
                                    if (0 <= x0 && x0 < image.Width) val += xRatio0 * oldStart0[y * oldWidth_3 + x0 * 3 + (x % 3)];
                                    if (0 <= x1 && x1 < image.Width) val += xRatio1 * oldStart0[y * oldWidth_3 + x1 * 3 + (x % 3)];
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
                    int leftTop = 0, rightTop = 0;
                    int smallerSide = Math.Min(image.Height, (int)(image.Height * scaleAbs));
                    if (Scale > 0 && scaleAbs >= 1 || Scale < 0 && scaleAbs < 1)
                        leftTop = (result.Height - smallerSide) / 2;
                    else
                        rightTop = (result.Height - smallerSide) / 2;
                    // Iterate through columns
                    Debug.Assert(image.Width == result.Width);
                    int width_3 = image.Width * 3;
                    for (int x = 0; x < width_3; ++x)
                    {
                        int x_div_3 = x / 3;
                        // Transform each row
                        float colOffset = leftTop + (rightTop - leftTop) * (x_div_3 / (result.Width - 1f));
                        float colScale = (result.Height - 2 * colOffset) / (float)image.Height;
                        Parallel.For(0, result.Height, y =>
                        {
                            float y_orig = (y - colOffset) / colScale;
                            int y0, y1;
                            switch (Interpolation)
                            {
                                case InterpolationMode.NearestNeighbor:
                                    y0 = (int)MathF.Round(y_orig);
                                    if (0 <= y0 && y0 < image.Height) newStart0[y * width_3 + x] = oldStart0[y0 * width_3 + x];
                                    break;
                                case InterpolationMode.Bilinear:
                                    y0 = (int)MathF.Floor(y_orig);
                                    y1 = (int)MathF.Ceiling(y_orig);
                                    float yRatio1 = y_orig - y0;
                                    float yRatio0 = 1 - yRatio1;
                                    float val = 0;
                                    if (0 <= y0 && y0 < image.Height) val += yRatio0 * oldStart0[y0 * width_3 + x];
                                    if (0 <= y1 && y1 < image.Height) val += yRatio1 * oldStart0[y1 * width_3 + x];
                                    newStart0[y * width_3 + x] = val.ClampToByte();
                                    break;
                                default:
                                    throw new ArgumentException($"Unknown interpolation mode: {Interpolation}");

                            }
                        });
                        reporter?.Report(x + 1, 0, width_3);
                    }
                }
                else throw new ArgumentException($"Unknown perspective direction: {Direction}.");

            }
            reporter?.Done();
            return result;
        }
    }
}
