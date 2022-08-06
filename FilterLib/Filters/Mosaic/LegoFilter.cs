using FilterLib.Reporting;
using FilterLib.Util;
using System;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Mosaic
{
    /// <summary>
    /// Pixelate the image and add circles as if it was made of Lego blocks.
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
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            int width_3 = image.Width * 3;
            int size_3 = size * 3;
            (float, byte)[,] oc = GetOuterCircle();
            float[,] ic = GetInnerCircle();
            fixed (byte* start = image)
            {
                byte* start0 = start;
                // Iterate through block rows
                int yMax = image.Height / size;
                if (yMax * size < image.Height) yMax++;
                Parallel.For(0, yMax, y =>
                {
                    y *= size;
                    // Iterate through block columns
                    for (int x = 0; x < width_3; x += size_3)
                    {
                        // Calculate average color in block
                        float rSum = 0, gSum = 0, bSum = 0;
                        int n = 0;
                        for (int ySub = 0; ySub < size && y + ySub < image.Height; ++ySub)
                        {
                            byte* row = start0 + ((y + ySub) * width_3);
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
                            byte* row = start0 + ((y + ySub) * width_3);
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
                            byte* row = start0 + ((y + ySub) * width_3);
                            for (int xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
                            {
                                float a = ic[xSub / 3, ySub];
                                row[x + xSub + 2] = (row[x + xSub + 2] * (1 - a) + rAvg * a).ClampToByte();
                                row[x + xSub + 1] = (row[x + xSub + 1] * (1 - a) + gAvg * a).ClampToByte();
                                row[x + xSub] = (row[x + xSub] * (1 - a) + bAvg * a).ClampToByte();
                            }
                        }
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, yMax);
                });
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
            float radius_squared = MathF.Pow(Size / 4f + 1, 2);
            int circleTop = Size / 4 - 1;
            int circleBottom = 3 * Size / 4 + 1;
            int topHalfFadeLength = Size / 4 + 1;
            int bottomHalfFadeLength = Size / 4;
            int bottomHalfStart = Size / 2 + 1;
            float center = Size / 2f;
            int half = size % 2 == 0 ? size / 2 : size / 2 + 1;

            // Calculate top half
            for (int x = 0; x < half; x++)
            {
                Parallel.For(0, Size, y =>
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
                });
            }
            // Mirror bottom half
            for (int x = half; x < Size; x++)
                Parallel.For(0, Size, y => map[x, y] = map[Size - x - 1, y]);

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
            float radius_squared = MathF.Pow(Size / 4f, 2);
            float center = Size / 2f;
            int half = size % 2 == 0 ? size / 2 : size / 2 + 1;

            // Calculate top half
            for (int x = 0; x < half; x++)
            {
                // Calculate left half
                Parallel.For(0, half, y =>
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
                    map[x, y] = samplesInside / (float)samplesTotal;
                });
                // Mirror right half
                Parallel.For(half, Size, y => map[x, y] = map[x, Size - y - 1]);
            }

            // Mirror bottom half
            for (int x = half; x < Size; x++)
                Parallel.For(0, Size, y => map[x, y] = map[Size - x - 1, y]);

            return map;
        }
    }
}
