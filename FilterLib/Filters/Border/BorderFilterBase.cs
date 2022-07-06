using FilterLib.Reporting;
using FilterLib.Util;
using System;

namespace FilterLib.Filters.Border
{
    /// <summary>
    /// Base class drawing a possibly rounded border around the picture where the
    /// concrete color is left for the derived classes.
    /// </summary>
    public abstract class BorderFilterBase : FilterBase
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
        /// Border position.
        /// </summary>
        [FilterParam]
        public BorderPosition Position { get; set; }

        /// <summary>
        /// Quality of anti-aliasing the rounded corners.
        /// </summary>
        [FilterParam]
        public AntiAliasQuality AntiAlias { get; set; }

        public BorderFilterBase() :
            this(Size.Absolute(0), Size.Absolute(0), BorderPosition.Inside, AntiAliasQuality.Medium)
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Border width</param>
        /// <param name="radius">Border radius</param>
        /// <param name="position">Border position</param>
        /// <param name="antiAlias">Quality of anti-aliasing the rounded corners</param>
        public BorderFilterBase(Size width, Size radius, BorderPosition position, AntiAliasQuality antiAlias)
        {
            Width = width;
            Radius = radius;
            Position = position;
            AntiAlias = antiAlias;
        }

        /// <inheritdoc/>
        public override sealed Image Apply(Image image, IReporter reporter = null)
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
            Image result = new(newWidth, newHeight);

            unsafe
            {
                fixed (byte* start = result, origStart = image)
            {
                    // Draw the image centered
                    int imgOffset = (newWidth - image.Width) / 2;
                    int imgOffset_3 = imgOffset * 3;
                    int width_3 = image.Width * 3;
                    for (int y = 0; y < image.Height; ++y)
                    {
                        byte* row = start + (y + imgOffset) * result.Width * 3;
                        byte* rowOrig = origStart + y * image.Width * 3;

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
                        byte* row = start + y * result.Width * 3;
                        for (int x = 0; x < newWidth_3; x += 3)
                            (row[x], row[x + 1], row[x+2]) = GetBorderAt(x / 3, y);
                    }
                    // Bottom
                    for (int y = Math.Max(0, newHeight - borderW); y < newHeight; ++y)
                    {
                        byte* row = start + y * result.Width * 3;
                        for (int x = 0; x < newWidth_3; x += 3)
                            (row[x ], row[x + 1], row[x+2]) = GetBorderAt(x / 3, y);
                    }
                    // Left and right
                    for (int y = borderW; y < newHeight - borderW; ++y)
                    {
                        byte* row = start + y * result.Width * 3;
                        // Left
                        for (int x = 0; x < borderW_3 && x < newWidth_3; x += 3)
                            (row[x ], row[x + 1], row[x+2]) = GetBorderAt(x / 3, y);
                        // Right
                        for (int x = Math.Max(0, newWidth_3 - borderW_3); x < newWidth_3; x += 3)
                            (row[x ], row[x + 1], row[x+2]) = GetBorderAt(x / 3, y);
                    }
                    reporter?.Report(67, 0, 100);

                    // Draw the circles (rounded corner)
                    int radius_2 = 2 * radius;
                    float[,] alphaMap = GenerateAlphaMap(radius);
                    for (int y = 0; y < radius && y + borderW < newHeight; ++y)
                    {
                        byte* row = start + (y + borderW) * result.Width * 3;
                        // Top left
                        for (int x = 0; x < radius && x + borderW < newWidth; ++x)
                        {
                            int x_3 = (x + borderW) * 3;
                            float a = alphaMap[x, y];
                            (byte r, byte g, byte b) = GetBorderAt(x + borderW, y + borderW);
                            row[x_3] = (a * r + (1 - a) * row[x_3]).ClampToByte();
                            row[x_3 + 1] = (a * g + (1 - a) * row[x_3 + 1]).ClampToByte();
                            row[x_3 + 2] = (a * b + (1 - a) * row[x_3 + 2]).ClampToByte();
                        }
                        // Top right
                        for (int x = radius_2 - 1; x >= radius && newWidth - borderW - radius_2 + x >= 0; --x)
                        {
                            int x_3 = (newWidth - borderW - radius_2 + x) * 3;
                            float a = alphaMap[x, y];
                            (byte r, byte g, byte b) = GetBorderAt(x_3 / 3, y + borderW);
                            row[x_3] = (a * r + (1 - a) * row[x_3]).ClampToByte();
                            row[x_3 + 1] = (a * g + (1 - a) * row[x_3 + 1]).ClampToByte();
                            row[x_3 + 2] = (a * b + (1 - a) * row[x_3 + 2]).ClampToByte();
                        }
                    }
                    for (int y = radius_2 - 1; y >= radius && newHeight - borderW - radius_2 + y >= 0; --y)
                    {
                        int yImg = newHeight - borderW - radius_2 + y;
                        byte* row = start + yImg * result.Width * 3;
                        // Bottom left
                        for (int x = 0; x < radius && x + borderW < newWidth; ++x)
                        {
                            int x_3 = (x + borderW) * 3;
                            float a = alphaMap[x, y];
                            (byte r, byte g, byte b) = GetBorderAt(x_3 / 3, yImg);
                            row[x_3] = (a * r + (1 - a) * row[x_3]).ClampToByte();
                            row[x_3 + 1] = (a * g + (1 - a) * row[x_3 + 1]).ClampToByte();
                            row[x_3 + 2] = (a * b + (1 - a) * row[x_3 + 2]).ClampToByte();
                        }
                        // Bottom right
                        for (int x = radius_2 - 1; x >= radius && newWidth - borderW - radius_2 + x >= 0; --x)
                        {
                            int x_3 = (newWidth - borderW - radius_2 + x) * 3;
                            float a = alphaMap[x, y];
                            (byte r, byte g, byte b) = GetBorderAt(x_3 / 3, yImg);
                            row[x_3] = (a * r + (1 - a) * row[x_3]).ClampToByte();
                            row[x_3 + 1] = (a * g + (1 - a) * row[x_3 + 1]).ClampToByte();
                            row[x_3 + 2] = (a * b + (1 - a) * row[x_3 + 2]).ClampToByte();
                        }
                    }
                    reporter?.Report(100, 0, 100);
                }
            }

            reporter?.Done();
            return result;
        }

        /// <summary>
        /// Get the color for the border at a given position.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>Color of the border</returns>
        protected abstract (byte, byte, byte) GetBorderAt(int x, int y);

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
