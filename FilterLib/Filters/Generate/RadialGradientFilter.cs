using FilterLib.Reporting;
using FilterLib.Util;
using System;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Generate
{
    /// <summary>
    /// Create a radial gradient with a given center and inner/outer radius. Everything inside
    /// the inner radius is black, everything outside the outer radius is black and there is
    /// a linear transition inbetween.
    /// </summary>
    [Filter]
    public sealed class RadialGradientFilter : FilterInPlaceBase
    {
        /// <summary>
        /// Center X coordinate.
        /// </summary>
        [FilterParam]
        public Size CenterX { get; set; }

        /// <summary>
        /// Center Y coordinate.
        /// </summary>
        [FilterParam]
        public Size CenterY { get; set; }

        /// <summary>
        /// Inner radius.
        /// </summary>
        [FilterParam]
        public Size InnerRadius { get; set; }

        /// <summary>
        /// Outer radius.
        /// </summary>
        [FilterParam]
        public Size OuterRadius { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="centerX">Center X coordinate</param>
        /// <param name="centerY">Center Y coordinate</param>
        /// <param name="innerRadius">Inner radius</param>
        /// <param name="outerRadius">Outer radius</param>
        public RadialGradientFilter(Size centerX, Size centerY, Size innerRadius, Size outerRadius)
        {
            CenterX = centerX;
            CenterY = centerY;
            InnerRadius = innerRadius;
            OuterRadius = outerRadius;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RadialGradientFilter() : this(Size.Relative(.5f), Size.Relative(.5f), Size.Relative(0), Size.Relative(.5f)) { }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            int cx = CenterX.ToAbsolute(image.Width);
            int cy = CenterY.ToAbsolute(image.Height);
            int ri = InnerRadius.ToAbsolute(Math.Min(image.Width, image.Height));
            int ro = OuterRadius.ToAbsolute(Math.Min(image.Width, image.Height));
            if (ro < ri) throw new ArgumentException("Outer radius must be greater or equal to inner radius.");
            int ri_2 = ri * ri;
            int ro_2 = ro * ro;

            fixed (byte* start = image)
            {
                byte* start0 = start;
                Parallel.For(0, image.Height, y =>
                {
                    byte* ptr = start0 + y * image.Width * 3;
                    for (int x = 0; x < image.Width; ++x)
                    {
                        int d_2 = (x - cx) * (x - cx) + (y - cy) * (y - cy);
                        if (d_2 <= ri_2) ptr[0] = ptr[1] = ptr[2] = 0;
                        else if (d_2 >= ro_2) ptr[0] = ptr[1] = ptr[2] = 255;
                        else ptr[0] = ptr[1] = ptr[2] = ((MathF.Sqrt(d_2) - ri) / (ro - ri) * 255).ClampToByte();
                        ptr += 3;
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }
            reporter?.Done();
        }
    }
}
