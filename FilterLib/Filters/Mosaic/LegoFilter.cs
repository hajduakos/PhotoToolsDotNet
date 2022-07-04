using FilterLib.Reporting;
using FilterLib.Util;
using System;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Filters.Mosaic
{
    /// <summary>
    /// Lego filter.
    /// </summary>
    [Filter]
    public sealed class LegoFilter : FilterInPlaceBase
    {
        private int size;

        /// <summary>
        /// Block size property [8;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(8)]
        public int Size
        {
            get { return size; }
            set { size = Math.Max(8, value); }
        }

        /// <summary>
        /// Quality of anti-aliasing.
        /// </summary>
        [FilterParam]
        public AntiAliasQuality AntiAlias { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="size">Block size</param>
        /// <param name="antiAlias">Quality of anti-aliasing</param>
        public LegoFilter(int size = 16, AntiAliasQuality antiAlias = AntiAliasQuality.Medium)
        {
            Size = size;
            AntiAlias = antiAlias;
        }

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb);
            int width_3 = image.Width * 3;
            int size_3 = size * 3;
            (float, byte)[,] oc = GetOuterCircle();
            float[,] ic = GetInnerCircle();
            unsafe
            {
                // Iterate through block rows
                for (int y = 0; y < image.Height; y += size)
                {
                    // Iterate through block columns
                    for (int x = 0; x < width_3; x += size_3)
                    {
                        // Calculate average color in block
                        int rSum = 0, gSum = 0, bSum = 0, n = 0;
                        for (int ySub = 0; ySub < size && y + ySub < image.Height; ++ySub)
                        {
                            byte* row = (byte*)bmd.Scan0 + ((y + ySub) * bmd.Stride);
                            for (int xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
                            {
                                rSum += row[x + xSub + 2];
                                gSum += row[x + xSub + 1];
                                bSum += row[x + xSub];
                                ++n;
                            }
                        }
                        byte rAvg = (rSum / n).ClampToByte();
                        byte gAvg = (gSum / n).ClampToByte();
                        byte bAvg = (bSum / n).ClampToByte();

                        // Use average color as basis and add outer circle on top of that
                        for (int ySub = 0; ySub < size && y + ySub < image.Height; ++ySub)
                        {
                            byte* row = (byte*)bmd.Scan0 + ((y + ySub) * bmd.Stride);
                            for (int xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
                            {
                                (float a, byte c) = oc[xSub / 3, ySub];
                                row[x + xSub + 2] = (rAvg * (1 - a) + c * a).ClampToByte();
                                row[x + xSub + 1] = (gAvg * (1 - a) + c * a).ClampToByte();
                                row[x + xSub] = (bAvg * (1 - a) + c * a).ClampToByte();
                            }
                        }

                        // Fill inner circle with average color
                        for (int ySub = 0; ySub < size && y + ySub < image.Height; ++ySub)
                        {
                            byte* row = (byte*)bmd.Scan0 + ((y + ySub) * bmd.Stride);
                            for (int xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
                            {
                                float a = ic[xSub / 3, ySub];
                                row[x + xSub + 2] = (row[x + xSub + 2] * (1 - a) + rAvg * a).ClampToByte();
                                row[x + xSub + 1] = (row[x + xSub + 1] * (1 - a) + gAvg * a).ClampToByte();
                                row[x + xSub] = (row[x + xSub] * (1 - a) + bAvg * a).ClampToByte();
                            }
                        }
                    }
                    reporter?.Report(y, 0, image.Height - 1);


                }

                reporter?.Done();
            }
        }

        /// <summary>
        /// Helper function to determine number of samples based on anti-aliasing quality.
        /// </summary>
        private int GetSampleCount()
        {
            return AntiAlias switch
            {
                AntiAliasQuality.None => 1,
                AntiAliasQuality.Low => 2,
                AntiAliasQuality.Medium => 4,
                AntiAliasQuality.High => 8,
                _ => throw new ArgumentException($"Unsupported anti-alias quality: {AntiAlias}"),
            };
        }

        /// <summary>
        /// Get (alpha value, color) pairs for the outer circles: a white fading circle on top half,
        /// and a black fading circle on the bottom.
        /// </summary>
        private (float, byte)[,] GetOuterCircle()
        {
            (float, byte)[,] map = new (float, byte)[Size, Size];
            int nSamples = GetSampleCount();
            float samplingDelta = nSamples == 1 ? 0 : 1f / (nSamples - 1);
            float radius_squared = MathF.Pow(Size / 4 + 1, 2);
            int circleTop = Size / 4 - 1;
            int circleBottom = 3 * Size / 4 + 1;
            int topHalfFadeLength = Size / 4 + 1;
            int bottomHalfFadeLength = Size / 4;
            int bottomHalfStart = Size / 2 + 1;
            float center = Size / 2f;

            // Loop through the whole box
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    // Create linear gradient first
                    float baseAlpha = 0; // Nothing is visible by default
                    byte color;
                    if (y < Size / 2)
                    {
                        // Top half: white color, fading from top to bottom
                        color = 255;
                        if (y >= circleTop) baseAlpha = 1 - (y - circleTop) / (float)topHalfFadeLength;
                    }
                    else
                    {
                        // Bottom half: black color fading from bottom to top
                        color = 0;
                        if (y >= bottomHalfStart && y < circleBottom) baseAlpha = (y - Size / 2) / (float)bottomHalfFadeLength;
                    }

                    // Then cut out an anti-aliased circle
                    int samplesInside = 0;
                    int samplesTotal = 0;
                    for (int dx = 0; dx < nSamples; ++dx)
                    {
                        for (int dy = 0; dy < nSamples; ++dy)
                        {
                            ++samplesTotal;
                            float x0 = x + (nSamples == 1 ? .5f : (dx * samplingDelta));
                            float y0 = y + (nSamples == 1 ? .5f : (dy * samplingDelta));
                            if ((x0 - center) * (x0 - center) + (y0 - center) * (y0 - center) <= radius_squared) ++samplesInside;
                        }
                    }
                    float circleAlpha = samplesInside / (float)samplesTotal;

                    map[x, y] = (baseAlpha * circleAlpha, color);
                }
            }

            return map;
        }

        /// <summary>
        /// Get anti-aliased alpha values corresponding to the inner circle.
        /// </summary>
        private float[,] GetInnerCircle()
        {
            float[,] map = new float[Size, Size];
            int nSamples = GetSampleCount();
            float samplingDelta = nSamples == 1 ? 0 : 1f / (nSamples - 1);
            float radius_squared = (Size / 4) * (Size / 4);
            float center = Size / 2f;

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    int samplesInside = 0;
                    int samplesTotal = 0;
                    for (int dx = 0; dx < nSamples; ++dx)
                    {
                        for (int dy = 0; dy < nSamples; ++dy)
                        {
                            ++samplesTotal;
                            float x0 = x + (nSamples == 1 ? .5f : (dx * samplingDelta));
                            float y0 = y + (nSamples == 1 ? .5f : (dy * samplingDelta));
                            if ((x0 - center) * (x0 - center) + (y0 - center) * (y0 - center) <= radius_squared) ++samplesInside;
                        }
                    }
                    map[x,y] = samplesInside / (float)samplesTotal;
                }
            }
            return map;
        }
    }
}
