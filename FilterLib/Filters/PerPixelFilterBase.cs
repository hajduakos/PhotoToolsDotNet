﻿using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using IReporter = FilterLib.Reporting.IReporter;
using FilterLib.Util;

namespace FilterLib.Filters
{
    /// <summary>
    /// Base class for filters that process each pixel individually.
    /// </summary>
    public abstract class PerPixelFilterBase : FilterInPlaceBase
    {
        /// <inheritdoc/>
        public override sealed void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            ApplyStart();
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                // Iterate through each pixel and process individually
                int width_3 = image.Width * 3;
                unsafe
                {
                    for (int y = 0; y < image.Height; ++y)
                    {
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        for (int x = 0; x < width_3; x += 3)
                        {
                            ProcessPixel(row + x + 2, row + x + 1, row + x);
                        }
                        reporter?.Report(y, 0, image.Height - 1);
                    }
                }
            }
            ApplyEnd();
            reporter?.Done();
        }

        /// <summary>
        /// Gets called when filter starts applying.
        /// </summary>
        protected virtual void ApplyStart() { }

        /// <summary>
        /// Gets called for each pixel independently.
        /// </summary>
        /// <param name="r">Pointer to red value</param>
        /// <param name="g">Pointer to green value</param>
        /// <param name="b">Pointer to blue value</param>
        protected abstract unsafe void ProcessPixel(byte* r, byte* g, byte* b);

        /// <summary>
        /// Gets called when filter finishes applying.
        /// </summary>
        protected virtual void ApplyEnd() { }
    }
}
