﻿using FilterLib.Reporting;
using FilterLib.Util;
using System;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Simple border filter.
    /// </summary>
    [Filter]
    public sealed class SimpleBorderFilter : FilterBase
    {
        /// <summary>
        /// Border width.
        /// </summary>
        [FilterParam]
        public Size Width { get; set; }

        /// <summary>
        /// Border radius.
        /// </summary>
        [FilterParam]
        public Size Radius { get; set; }

        /// <summary>
        /// Border color.
        /// </summary>
        [FilterParam]
        public RGB Color { get; set; }

        /// <summary>
        /// Border position.
        /// </summary>
        [FilterParam]
        public BorderPosition Position { get; set; }

        /// <summary>
        /// Quality of anti-aliasing the rounded corners.
        /// </summary>
        [FilterParam]
        public AntiAliasQuality AntiAlias { get; set; }

        public SimpleBorderFilter() :
            this(Size.Absolute(0), Size.Absolute(0), new RGB(0, 0, 0), BorderPosition.Inside, AntiAliasQuality.Medium) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Border width</param>
        /// <param name="radius">Border radius</param>
        /// <param name="color">Border color</param>
        /// <param name="position">Border position</param>
        /// <param name="antiAlias">Quality of anti-aliasing the rounded corners</param>
        public SimpleBorderFilter(Size width, Size radius, RGB color, BorderPosition position, AntiAliasQuality antiAlias)
        {
            Width = width;
            Radius = radius;
            Color = color;
            Position = position;
            AntiAlias = antiAlias;
        }

        /// <inheritdoc/>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            int borderW = Width.ToAbsolute(Math.Max(image.Width, image.Height));
            int radius = Radius.ToAbsolute(Math.Max(image.Width, image.Height));
            (int newWidth, int newHeight) = Position switch
            {
                BorderPosition.Inside => (image.Width, image.Height),
                BorderPosition.Center => (image.Width + borderW, image.Height + borderW),
                BorderPosition.Outside => (image.Width + 2 * borderW, image.Height + 2 * borderW),
                _ => throw new ArgumentException($"Unknown border position: {Position}.")
            };
            Bitmap result = new(newWidth, newHeight);
            using (DisposableBitmapData bmdOrig = new(image, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmd = new(result, PixelFormat.Format24bppRgb))
            {
                unsafe
                {
                    // Draw the image centered
                    int imgOffset = (newWidth - image.Width) / 2;
                    int imgOffset_3 = imgOffset * 3;
                    int width_3 = image.Width * 3;
                    for (int y = 0; y < image.Height; ++y)
                    {
                        byte* row = (byte*)bmd.Scan0 + (y + imgOffset) * bmd.Stride;
                        byte* rowOrig = (byte*)bmdOrig.Scan0 + y * bmdOrig.Stride;

                        for (int x = 0; x < width_3; x += 3)
                        {
                            row[x + imgOffset_3] = rowOrig[x];
                            row[x + imgOffset_3 + 1] = rowOrig[x + 1];
                            row[x + imgOffset_3 + 2] = rowOrig[x + 2];
                        }
                    }
                    reporter?.Report(33, 0, 100);

                    // Draw the 4 borders around the image
                    int newWidth_3 = newWidth * 3;
                    int borderW_3 = borderW * 3;
                    // Top
                    for (int y = 0; y < borderW && y < newHeight; ++y)
                    {
                        byte* row = (byte*)bmd.Scan0 + y * bmd.Stride;
                        for (int x = 0; x < newWidth_3; x += 3)
                            (row[x+2], row[x+1], row[x]) = (Color.R, Color.G, Color.B);
                    }
                    // Bottom
                    for (int y = Math.Max(0, newHeight - borderW); y < newHeight; ++y)
                    {
                        byte* row = (byte*)bmd.Scan0 + y * bmd.Stride;
                        for (int x = 0; x < newWidth_3; x += 3)
                            (row[x + 2], row[x + 1], row[x]) = (Color.R, Color.G, Color.B);
                    }
                    // Left and right
                    for (int y = borderW; y < newHeight - borderW; ++y)
                    {
                        byte* row = (byte*)bmd.Scan0 + y * bmd.Stride;
                        // Left
                        for (int x = 0; x < borderW_3 && x < newWidth_3; x += 3)
                            (row[x + 2], row[x + 1], row[x]) = (Color.R, Color.G, Color.B);
                        // Right
                        for (int x = Math.Max(0, newWidth_3 - borderW_3); x < newWidth_3; x += 3)
                            (row[x + 2], row[x + 1], row[x]) = (Color.R, Color.G, Color.B);
                    }
                    reporter?.Report(67, 0, 100);

                    // Draw the circles (rounded corner)
                    int radius_2 = 2 * radius;
                    float[,] alphaMap = GenerateAlphaMap(radius);
                    for (int y = 0; y < radius && y + borderW < newHeight; ++y)
                    {
                        byte* row = (byte*)bmd.Scan0 + (y + borderW) * bmd.Stride;
                        // Top left
                        for (int x = 0; x < radius && x + borderW < newWidth; ++x)
                        {
                            int x_3 = (x + borderW) * 3;
                            float a = alphaMap[x, y];
                            row[x_3] = (a * Color.B + (1 - a) * row[x_3]).ClampToByte();
                            row[x_3 + 1] = (a * Color.G + (1 - a) * row[x_3 + 1]).ClampToByte();
                            row[x_3 + 2] = (a * Color.R + (1 - a) * row[x_3 + 2]).ClampToByte();
                        }
                        // Top right
                        for (int x = radius_2 - 1; x >= radius && newWidth - borderW - radius_2 + x >= 0; --x)
                        {
                            int x_3 = (newWidth - borderW - radius_2 + x) * 3;
                            float a = alphaMap[x, y];
                            row[x_3] = (a * Color.B + (1 - a) * row[x_3]).ClampToByte();
                            row[x_3 + 1] = (a * Color.G + (1 - a) * row[x_3 + 1]).ClampToByte();
                            row[x_3 + 2] = (a * Color.R + (1 - a) * row[x_3 + 2]).ClampToByte();
                        }
                    }
                    for (int y = radius_2 - 1; y >= radius && newHeight - borderW - radius_2 + y >= 0; --y)
                    {
                        byte* row = (byte*)bmd.Scan0 + (newHeight - borderW - radius_2 + y) * bmd.Stride;
                        // Bottom left
                        for (int x = 0; x < radius && x + borderW < newWidth; ++x)
                        {
                            int x_3 = (x + borderW) * 3;
                            float a = alphaMap[x, y];
                            row[x_3] = (a * Color.B + (1 - a) * row[x_3]).ClampToByte();
                            row[x_3 + 1] = (a * Color.G + (1 - a) * row[x_3 + 1]).ClampToByte();
                            row[x_3 + 2] = (a * Color.R + (1 - a) * row[x_3 + 2]).ClampToByte();
                        }
                        // Bottom right
                        for (int x = radius_2 - 1; x >= radius && newWidth - borderW - radius_2 + x >= 0; --x)
                        {
                            int x_3 = (newWidth - borderW - radius_2 + x) * 3;
                            float a = alphaMap[x, y];
                            row[x_3] = (a * Color.B + (1 - a) * row[x_3]).ClampToByte();
                            row[x_3 + 1] = (a * Color.G + (1 - a) * row[x_3 + 1]).ClampToByte();
                            row[x_3 + 2] = (a * Color.R + (1 - a) * row[x_3 + 2]).ClampToByte();
                        }
                    }
                    reporter?.Report(100, 0, 100);
                }
            }

            reporter?.Done();
            return result;
        }

        private float[,] GenerateAlphaMap(int radius)
        {
            var samples = AntiAlias switch
            {
                AntiAliasQuality.None => 1,
                AntiAliasQuality.Low => 2,
                AntiAliasQuality.Medium => 4,
                AntiAliasQuality.High => 8,
                _ => throw new ArgumentException($"Unsupported anti-alias quality: {AntiAlias}"),
            };
            float delta = samples == 1 ? 0 : 1f / (samples - 1);
            int radius_2 = radius * 2;
            float[,] map = new float[radius_2, radius_2];
            for (int x = 0; x < radius_2; ++x)
            {
                for (int y = 0; y < radius_2; ++y)
                {
                    int outside = 0;
                    int total = 0;
                    for (int dx = 0; dx < samples; ++dx)
                    {
                        for (int dy = 0; dy < samples; ++dy)
                        {
                            ++total;
                            float x0 = x + (samples == 1 ? .5f : (dx * delta));
                            float y0 = y + (samples == 1 ? .5f : (dy * delta));
                            if ((x0 - radius) * (x0 - radius) + (y0 - radius) * (y0 - radius) > radius * radius) ++outside;
                        }
                    }
                    map[x, y] = outside / (float)total;
                }
            }
            return map;
        }
    }
}
