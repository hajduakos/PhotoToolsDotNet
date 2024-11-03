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
    public class SkewFilter : FilterBase
    {
        /// <summary>
        /// Skew direction.
        /// </summary>
        public enum SkewDirection { Horizontal, Vertical }

        private float angle;

        /// <summary>
        /// Skew angle in degrees ]-90;90[.
        /// </summary>
        [FilterParam]
        [FilterParamMinF(-90)]
        [FilterParamMaxF(90)]
        public float Angle
        {
            get { return angle; }
            set { angle = value.Clamp(-89.99f, 89.99f); }
        }

        /// <summary>
        /// Direction.
        /// </summary>
        [FilterParam]
        public SkewDirection Direction { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="angle">Skew angle in degrees ]-90;90[</param>
        public SkewFilter(float angle = 0, SkewDirection direction = SkewDirection.Horizontal)
        {
            Angle = angle;
            Direction = direction;
        }

        public override unsafe Image Apply(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            float angleRad = Angle * MathF.PI / 180f;
            float angleTan = MathF.Tan(angleRad);
            Image result;
            if (Direction == SkewDirection.Horizontal)
                result = new(image.Width + (int)Math.Round(image.Height * MathF.Abs(angleTan)), image.Height);
            else if (Direction == SkewDirection.Vertical)
                result = new(image.Width, image.Height + (int)Math.Round(image.Width * MathF.Abs(angleTan)));
            else
                throw new ArgumentException($"Unknown skew direction: {Direction}.");

            fixed (byte* oldStart = image, newStart = result)
            {
                byte* newStart0 = newStart;
                byte* oldStart0 = oldStart;
                if (Direction == SkewDirection.Horizontal)
                {
                    int newWidth_3 = result.Width * 3;
                    int oldWidth_3 = image.Width * 3;
                    // Iterate through rows
                    Debug.Assert(image.Height == result.Height);
                    int offset_3 = Angle < 0 ? (int)(image.Height * MathF.Abs(angleTan)) * 3 : 0;
                    for (int y = 0; y < result.Height; ++y)
                    {
                        // Skew each column
                        int dx_3 = (int)(y * angleTan) * 3;
                        Parallel.For(0, newWidth_3, x =>
                        {
                            int old_x = x - dx_3 - offset_3;
                            if (0 <= old_x && old_x < oldWidth_3) newStart0[y * newWidth_3 + x] = oldStart0[y * oldWidth_3 + old_x];
                        });
                        reporter?.Report(y + 1, 0, result.Height);
                    }
                }
                else if (Direction == SkewDirection.Vertical)
                {
                    int width_3 = image.Width * 3;
                    // Iterate through columns
                    Debug.Assert(image.Width == result.Width);
                    int offset = Angle < 0 ? (int)(image.Width * MathF.Abs(angleTan)) : 0;
                    for (int x = 0; x < width_3; ++x)
                    {
                        // Skew each row
                        int x_div_3 = x / 3;
                        int dy = (int)(x_div_3 * angleTan);
                        Parallel.For(0, result.Height, y =>
                        {
                            int old_y = y - dy - offset;
                            if (0 <= old_y && old_y < image.Height) newStart0[y * width_3 + x] = oldStart0[old_y * width_3 + x];
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
