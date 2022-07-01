﻿using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Crop filter.
    /// </summary>
    [Filter]
    public sealed class CropFilter : FilterBase
    {
        /// <summary>
        /// Crop area left.
        /// </summary>
        [FilterParam]
        public Util.Size X { get; set; }

        /// <summary>
        /// Crop area top.
        /// </summary>
        [FilterParam]
        public Util.Size Y { get; set; }

        /// <summary>
        /// Crop area width.
        /// </summary>
        [FilterParam]
        public Util.Size Width { get; set; }

        /// <summary>
        /// Crop area height.
        /// </summary>
        [FilterParam]
        public Util.Size Height { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CropFilter() : this(
            Util.Size.Absolute(0),
            Util.Size.Absolute(0),
            Util.Size.Relative(1),
            Util.Size.Relative(1))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Crop area left</param>
        /// <param name="y">Crop area top</param>
        /// <param name="width">Crop area width</param>
        /// <param name="height">Crop area height</param>
        public CropFilter(Util.Size x, Util.Size y, Util.Size width, Util.Size height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            int x0 = X.ToAbsolute(image.Width);
            int y0 = Y.ToAbsolute(image.Height);
            int w0 = Width.ToAbsolute(image.Width);
            int h0 = Height.ToAbsolute(image.Height);
            if (x0 < 0) throw new ArgumentException($"Ivalid X: {x0}");
            if (y0 < 0) throw new ArgumentException($"Ivalid Y: {y0}");
            if (w0 <= 0) throw new ArgumentException($"Ivalid Width: {w0}");
            if (h0 <= 0) throw new ArgumentException($"Ivalid Height: {h0}");
            w0 = Math.Min(w0, image.Width);
            h0 = Math.Min(h0, image.Height);

            Bitmap cropped = new(w0, h0);
            int x, y;
            int wMul3 = cropped.Width * 3;
            int x0Mul3 = x0 * 3;
            using (DisposableBitmapData bmd = new(cropped, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdOrg = new(image, PixelFormat.Format24bppRgb))
            {
                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h0; ++y)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        byte* rowOrg = (byte*)bmdOrg.Scan0 + ((y + y0) * bmdOrg.Stride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                        {
                            row[x] = rowOrg[x0Mul3 + x];
                            row[x + 1] = rowOrg[x0Mul3 + x + 1];
                            row[x + 2] = rowOrg[x0Mul3 + x + 2];
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, h0 - 1);
                    }
                }
            }
            reporter?.Done();
            return cropped;
        }
    }
}
