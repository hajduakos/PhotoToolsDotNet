using FilterLib.Reporting;
using FilterLib.Util;
using System;

namespace FilterLib.Filters.Generate
{
    /// <summary>
    /// Generate a linear gradient from black to white defined by a start and an end point.
    /// </summary>
    [Filter]
    public sealed class LinearGradientFilter : FilterInPlaceBase
    {
        /// <summary>
        /// X coordinate of the start point.
        /// </summary>
        [FilterParam]
        public Size StartX { get; set; }

        /// <summary>
        /// Y coordinate of the start point.
        /// </summary>
        [FilterParam]
        public Size StartY { get; set; }

        /// <summary>
        /// X coordinate of the end point.
        /// </summary>
        [FilterParam]
        public Size EndX { get; set; }

        /// <summary>
        /// Y coordinate of the end point.
        /// </summary>
        [FilterParam]
        public Size EndY { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="startX">X coordinate of the start point</param>
        /// <param name="startY">Y coordinate of the start point</param>
        /// <param name="endX">X coordinate of the end point</param>
        /// <param name="endY">Y coordinate of the end point</param>
        public LinearGradientFilter(Size startX, Size startY, Size endX, Size endY)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LinearGradientFilter() : this(Size.Relative(0), Size.Relative(0), Size.Relative(0), Size.Relative(1)) { }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            int x1 = StartX.ToAbsolute(image.Width);
            int x2 = EndX.ToAbsolute(image.Width);
            int y1 = StartY.ToAbsolute(image.Height);
            int y2 = EndY.ToAbsolute(image.Height);
            int len = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
            if (len == 0) throw new ArgumentException("Start and endpoints are the same.");
            float lenF = len;

            fixed(byte* start = image)
            {
                byte* ptr = start;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        // t corresponds the projection of the current point to the line
                        // defined by (x1,y1) -> (x2,y2). A value of 0 corresponds to (x1,y1),
                        // a value of 1 corresponds to (x2,y2). Smaller than 0 or greater than 1
                        // represent points outside the segment, which will be simply clamped.
                        float t = ((x - x1) * (x2 - x1) + (y - y1) * (y2 - y1)) / lenF;
                        ptr[0] = ptr[1] = ptr[2] = (t * 255).ClampToByte();
                        ptr += 3;
                    }
                    reporter?.Report(y + 1, 0, image.Height);
                }
            }
            reporter?.Done();
        }
    }
}
