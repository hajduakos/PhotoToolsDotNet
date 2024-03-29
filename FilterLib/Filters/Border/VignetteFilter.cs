﻿using FilterLib.Reporting;
using FilterLib.Util;
using System;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Add an ellipse shaped vignette that gradually ades off towards the center.
    /// </summary>
    [Filter]
    public sealed class VignetteFilter : FilterInPlaceBase
    {

        /// <summary>
        /// Radius of the vignette, outside is filled with given color.
        /// </summary>
        [FilterParam]
        public Size Radius { get; set; }

        /// <summary>
        /// Radius of the clear zone.
        /// </summary>
        [FilterParam]
        public Size ClearRadius { get; set; }

        /// <summary>
        /// Vignette color.
        /// </summary>
        [FilterParam]
        public RGB Color { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public VignetteFilter() : this(Size.Relative(3), Size.Relative(2), new RGB(0, 0, 0)) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="radius">Radius of the vignette</param>
        /// <param name="clearRadius">Radius of the clear zone</param>
        /// <param name="color">Color of the vignette</param>
        public VignetteFilter(Size radius, Size clearRadius, RGB color)
        {
            Radius = radius;
            ClearRadius = clearRadius;
            Color = color;
        }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            float a1 = Radius.ToAbsolute(image.Width / 2);
            float a0 = ClearRadius.ToAbsolute(image.Width / 2);
            if (a1 < a0) throw new ArgumentException("Radius must be larger than clear zone radius.");
            float halfWidth = image.Width / 2f, halfHeight = image.Height / 2f;
            float ratioSquare = image.Width * image.Width / (float)(image.Height * image.Height);
            float normalizer = MathF.PI / (a1 - a0);

            fixed (byte* start = image)
            {
                byte* start0 = start;
                Parallel.For(0, image.Height, y =>
                {
                    byte* ptr = start0 + y * image.Width * 3;
                    for (int x = 0; x < image.Width; ++x)
                    {
                        // Calculate coordinates with the origin at the center
                        float xShifted = x - halfWidth;
                        float yShifted = y - halfHeight;
                        // Calculate the radius (A) of the ellipse on which the point is
                        float ellipseRadius = MathF.Sqrt(xShifted * xShifted + yShifted * yShifted * ratioSquare);
                        // If the point is outside the vignette area, set the color to given one
                        if (ellipseRadius > a1)
                        {
                            ptr[0] = Color.R;
                            ptr[1] = Color.G;
                            ptr[2] = Color.B;
                        }
                        // Else if the point is outside the clear zone, calculate opacity
                        else if (ellipseRadius >= a0)
                        {
                            float op = MathF.Cos((ellipseRadius - a0) * normalizer) / 2f + 0.5f; // Cosine transition
                            //op = 1- (a_tmp - a0) / (a1 - a0); // Linear transition
                            ptr[0] = (byte)(ptr[0] * op + Color.R * (1 - op));
                            ptr[1] = (byte)(ptr[1] * op + Color.G * (1 - op));
                            ptr[2] = (byte)(ptr[2] * op + Color.B * (1 - op));
                        }
                        ptr += 3;
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }
            reporter?.Done();
        }
    }
}
