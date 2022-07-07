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
        public override sealed unsafe Image Apply(Image image, IReporter reporter = null)
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
            fixed (byte* newStart = result, oldStart = image)
            {
                // Draw the image centered
                int imgOffset = (newWidth - image.Width) / 2;
                int imgOffset_3 = imgOffset * 3;
                int oldWidth_3 = image.Width * 3;
                int newWidth_3 = result.Width * 3;
                for (int y = 0; y < image.Height; ++y)
                {
                    byte* newPtr = newStart + (y + imgOffset) * newWidth_3;
                    byte* oldPtr = oldStart + y * oldWidth_3;

                    for (int x = 0; x < image.Width; ++x)
                    {
                        newPtr[imgOffset_3] = oldPtr[0];
                        newPtr[imgOffset_3 + 1] = oldPtr[1];
                        newPtr[imgOffset_3 + 2] = oldPtr[2];
                        oldPtr += 3;
                        newPtr += 3;
                    }
                }
                reporter?.Report(33, 0, 100);

                // Draw the 4 borders around the image
                int borderW_3 = borderW * 3;
                // Top
                byte* ptr = newStart;
                for (int y = 0; y < borderW && y < newHeight; ++y)
                {
                    for (int x = 0; x < result.Width; ++x)
                    {
                        (ptr[0], ptr[1], ptr[2]) = GetBorderAt(x, y);
                        ptr += 3;
                    }
                }
                // Bottom
                int y0 = Math.Max(0, newHeight - borderW);
                ptr = newStart + y0 * newWidth_3;
                for (int y = y0; y < newHeight; ++y)
                {
                    for (int x = 0; x < result.Width; ++x)
                    {
                        (ptr[0], ptr[1], ptr[2]) = GetBorderAt(x, y);
                        ptr += 3;
                    }
                }
                // Left and right
                for (int y = borderW; y < newHeight - borderW; ++y)
                {
                    ptr = newStart + y * newWidth_3;
                    // Left
                    for (int x = 0; x < borderW && x < result.Width; ++x)
                    {
                        (ptr[0], ptr[1], ptr[2]) = GetBorderAt(x, y);
                        ptr += 3;
                    }
                    // Right
                    int x0 = Math.Max(0, result.Width - borderW);
                    ptr = newStart + y * newWidth_3 + x0 * 3;
                    for (int x = x0; x < result.Width; ++x)
                    {
                        (ptr[0], ptr[1], ptr[2]) = GetBorderAt(x, y);
                        ptr += 3;
                    }
                }
                reporter?.Report(67, 0, 100);

                // Draw the circles (rounded corner)
                int radius_2 = 2 * radius;
                float[,] alphaMap = GenerateAlphaMap(radius);
                for (int y = 0; y < radius && y + borderW < newHeight; ++y)
                {
                    byte* row = newStart + (y + borderW) * newWidth_3;
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
                    byte* row = newStart + yImg * newWidth_3;
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
