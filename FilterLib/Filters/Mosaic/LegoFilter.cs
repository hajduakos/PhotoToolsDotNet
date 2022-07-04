using FilterLib.Reporting;
using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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
            int h = image.Height;
            int x, y, xSub, ySub, size_3 = size * 3, rSum, gSum, bSum, n;
            unsafe
            {
                // Iterate through block rows
                for (y = 0; y < h; y += size)
                {
                    // Iterate through block columns
                    for (x = 0; x < width_3; x += size_3)
                    {
                        rSum = gSum = bSum = n = 0; // Clear sums
                        for (ySub = 0; ySub < size && y + ySub < h; ++ySub)
                        {
                            // Get row
                            byte* row = (byte*)bmd.Scan0 + ((y + ySub) * bmd.Stride);
                            for (xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
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
                        (float, byte)[,] oc = GetOuterCircle();
                        for (ySub = 0; ySub < size && y + ySub < h; ++ySub)
                        {
                            // Get row
                            byte* row = (byte*)bmd.Scan0 + ((y + ySub) * bmd.Stride);
                            for (xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
                            {
                                (float a, byte c) = oc[xSub / 3, ySub];
                                row[x + xSub + 2] = (rAvg * (1 - a) + c * a).ClampToByte();
                                row[x + xSub + 1] = (gAvg * (1 - a) + c * a).ClampToByte();
                                row[x + xSub] = (bAvg * (1 - a) + c * a).ClampToByte();
                            }
                        }

                        float[,] ic = GetInnerCircle();
                        for (ySub = 0; ySub < size && y + ySub < h; ++ySub)
                        {
                            // Get row
                            byte* row = (byte*)bmd.Scan0 + ((y + ySub) * bmd.Stride);
                            for (xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
                            {
                                float a = ic[xSub / 3, ySub];
                                row[x + xSub + 2] = (row[x + xSub + 2] * (1 - a) + rAvg * a).ClampToByte();
                                row[x + xSub + 1] = (row[x + xSub + 1] * (1 - a) + gAvg * a).ClampToByte();
                                row[x + xSub] = (row[x + xSub] * (1 - a) + bAvg * a).ClampToByte();
                            }
                        }
                    }
                    reporter?.Report(y, 0, h - 1);


                }

                reporter?.Done();
            }
        }

        private int GetSamples()
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

        private (float, byte)[,] GetOuterCircle()
        {
            (float, byte)[,] map = new (float, byte)[Size, Size];
            int samples = GetSamples();
            float delta = samples == 1 ? 0 : 1f / (samples - 1);
            float r = Size / 4 + 1;
            float c = Size / 2f;

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    byte color;
                    float baseAlpha = 0;
                    if (y < Size / 2)
                    {
                        color = 255;
                        if (y >= Size / 4 - 1) baseAlpha = 1 - (y - (Size / 4 - 1)) / (float)(Size / 4 + 1);
                    }
                    else
                    {
                        color = 0;
                        if (y >= Size / 2 + 1 && y < 3 * Size / 4 + 1) baseAlpha = (y - Size / 2) / (float)(Size / 4);
                    }

                    int outside = 0;
                    int total = 0;
                    for (int dx = 0; dx < samples; ++dx)
                    {
                        for (int dy = 0; dy < samples; ++dy)
                        {
                            ++total;
                            float x0 = x + (samples == 1 ? .5f : (dx * delta));
                            float y0 = y + (samples == 1 ? .5f : (dy * delta));
                            if ((x0 - c) * (x0 - c) + (y0 - c) * (y0 - c) > r * r) ++outside;
                        }
                    }
                    float circleAlpha = 1 - outside / (float)total;

                    map[x, y] = (baseAlpha * circleAlpha, color);
                }
            }

            return map;
        }

        private float[,] GetInnerCircle()
        {
            float[,] map = new float[Size, Size];
            int samples = GetSamples();
            float delta = samples == 1 ? 0 : 1f / (samples - 1);
            float r = Size / 4;
            float c = Size / 2f;

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
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
                            if ((x0 - c) * (x0 - c) + (y0 - c) * (y0 - c) > r * r) ++outside;
                        }
                    }
                    map[x,y] = 1 - outside / (float)total;
                }
            }
            return map;
        }
    }
}
