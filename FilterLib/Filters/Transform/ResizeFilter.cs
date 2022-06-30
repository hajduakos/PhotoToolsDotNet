﻿using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Resize filter.
    /// </summary>
    [Filter]
    public sealed class ResizeFilter : FilterBase
    {
        public enum InterpolationMode { NearestNeighbor }

        /// <summary>
        /// New width.
        /// </summary>
        [FilterParam]
        public Util.Size Width { get; set; }

        /// <summary>
        /// New height.
        /// </summary>
        [FilterParam]
        public Util.Size Height { get; set; }

        /// <summary>
        /// Interpolation mode.
        /// </summary>
        [FilterParam]
        public InterpolationMode Interpolation { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        /// <param name="interpolation">Interpolation mode</param>
        public ResizeFilter(Util.Size width, Util.Size height, InterpolationMode interpolation = InterpolationMode.NearestNeighbor)
        {
            this.Width = width;
            this.Height = height;
            this.Interpolation = interpolation;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ResizeFilter() : this(Util.Size.Relative(1f), Util.Size.Relative(1f)) { }

        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            int newWidth = Width.ToAbsolute(image.Width);
            int newHeight = Height.ToAbsolute(image.Height);

            if (newWidth <= 0) throw new ArgumentException($"Invalid new width: {newWidth}.");
            if (newHeight <= 0) throw new ArgumentException($"Invalid new height: {newHeight}.");

            Bitmap resized = new(newWidth, newHeight);
            using (DisposableBitmapData bmd = new(resized, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdOrg = new(image, PixelFormat.Format24bppRgb))
            {
                int wMul3 = resized.Width * 3;
                int h = resized.Height;
                int x, y;
                int x0, y0;
                // For sampling purposes, we assume that a pixel represents the color in the middle of
                // the pixel. For example, for an image with width 3, pixels at 0, 1, 2 represent the
                // colors at 0.5, 1.5 and 2.5 respectively. Hence the +0.5 and -0.5 adjustments.
                float wScale = image.Width / (float)resized.Width;
                float hScale = image.Height / (float)resized.Height;

                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get row
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                        {
                            switch (Interpolation)
                            {
                                case InterpolationMode.NearestNeighbor:
                                    y0 = (int)Math.Round((y + .5f) * hScale - .5f);
                                    x0 = (int)Math.Round((x / 3 + .5f) * wScale - .5f) * 3;
                                    byte* rowOrg = (byte*)bmdOrg.Scan0 + (y0 * bmdOrg.Stride);
                                    row[x] = rowOrg[x0];
                                    row[x + 1] = rowOrg[x0 + 1];
                                    row[x + 2] = rowOrg[x0 + 2];
                                    break;
                                default:
                                    throw new ArgumentException($"Unknown interpolation mode: {Interpolation}");
                            }
                        }
                    }
                }
            }

            reporter?.Done();
            return resized;
        }
    }
}