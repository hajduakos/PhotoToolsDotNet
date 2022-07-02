using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Random = System.Random;

namespace FilterLib.Filters.Mosaic
{
    /// <summary>
    /// Crystallize filter.
    /// </summary>
    [Filter]
    public sealed class CrystallizeFilter : FilterInPlaceBase
    {
        private int size, averaging;

        /// <summary>
        /// Crystal size [1;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(1)]
        public int Size
        {
            get { return size; }
            set { size = Math.Max(1, value); }
        }

        /// <summary>
        /// Averaging strength [0;100].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(100)]
        public int Averaging
        {
            get { return averaging; }
            set { averaging = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Random number generator seed.
        /// </summary>
        [FilterParam]
        public int Seed { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="size">Crystal size [1;...]</param>
        /// <param name="averaging">Averaging strength [0;100]</param>
        /// <param name="seed">Random generator seed</param>
        public CrystallizeFilter(int size = 1, int averaging = 100, int seed = 0)
        {
            Size = size;
            Averaging = averaging;
            Seed = seed;
        }


        /// <summary>
        /// Private helper class representing a point
        /// </summary>
        private struct Cpoint
        {
            public int x, y; // Coordinates

            // Constructor
            public Cpoint(int x = 0, int y = 0) { this.x = x; this.y = y; }

            // Get distance from other point (without square root)
            public int Dist(int x1, int y1) => (x - x1) * (x - x1) + (y - y1) * (y - y1);
        }

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (DisposableBitmapData bmd = new(image, PixelFormat.Format24bppRgb))
            {
                int w = image.Width;
                int width_3 = image.Width * 3; // Width of a row
                int h = image.Height;
                int x, y, xSub, ySub, size_3 = size * 3, i_3;
                int stride = bmd.Stride;
                float avg = averaging / 100.0f;

                // Generate points
                Random rnd = new(Seed);
                // Additional points required if the width/height is not dividible by the size
                int crystalsX = w / size + ((w % size) == 0 ? 0 : 1);
                int crystalsY = h / size + ((h % size) == 0 ? 0 : 1);
                Cpoint[,] crystalPoints = new Cpoint[crystalsX, crystalsY];
                for (xSub = 0; xSub < crystalsX; ++xSub)
                {
                    for (ySub = 0; ySub < crystalsY; ++ySub)
                    {
                        // Generate random points
                        crystalPoints[xSub, ySub] = new Cpoint(xSub * size + rnd.Next() % size, ySub * size + rnd.Next() % size);
                        // Check bounds
                        crystalPoints[xSub, ySub].x = Math.Min(crystalPoints[xSub, ySub].x, w - 1);
                        crystalPoints[xSub, ySub].y = Math.Min(crystalPoints[xSub, ySub].y, h - 1);
                    }
                }

                // Average colors of the points
                byte[,,] avgColors = new byte[crystalsX, crystalsY, 3];
                int rSum, gSum, bSum, n;

                int blockX, blockY; // Current block coordinates
                unsafe
                {
                    byte* imgStartPtr = (byte*)bmd.Scan0;

                    // First step: averaging
                    for (y = 0; y < h; y += size)
                    {
                        for (x = 0; x < width_3; x += size_3)
                        {
                            rSum = gSum = bSum = n = 0; // Clear sums
                            for (ySub = 0; ySub < size && y + ySub < h; ++ySub)
                            {
                                for (xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
                                {
                                    rSum += imgStartPtr[(y + ySub) * stride + x + xSub + 2];
                                    gSum += imgStartPtr[(y + ySub) * stride + x + xSub + 1];
                                    bSum += imgStartPtr[(y + ySub) * stride + x + xSub];
                                    ++n;
                                }
                            }
                            // Get representative point
                            Cpoint pnt = crystalPoints[x / size_3, y / size];
                            // Set average color as a combination of the average color and the representative point
                            avgColors[x / size_3, y / size, 0] = (byte)(avg * bSum / n + (1 - avg) * imgStartPtr[pnt.y * stride + pnt.x * 3]);
                            avgColors[x / size_3, y / size, 1] = (byte)(avg * gSum / n + (1 - avg) * imgStartPtr[pnt.y * stride + pnt.x * 3 + 1]);
                            avgColors[x / size_3, y / size, 2] = (byte)(avg * rSum / n + (1 - avg) * imgStartPtr[pnt.y * stride + pnt.x * 3 + 2]);
                        }
                        if ((y & 63) == 0) reporter?.Report(y, 0, 2 * h - 1);
                    }


                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Iterate through columns
                        for (x = 0; x < width_3; x += 3)
                        {
                            i_3 = x / 3;
                            // Get current block
                            blockX = i_3 / size;
                            blockY = y / size;
                            // Find closest point in the surrounding 5x5 square of blocks
                            Cpoint minPoint = crystalPoints[blockX, blockY];
                            int minDistance = minPoint.Dist(i_3, y);
                            for (xSub = Math.Max(0, blockX - 2); xSub <= blockX + 2 && xSub < crystalsX; ++xSub)
                            {
                                for (ySub = Math.Max(0, blockY - 2); ySub <= blockY + 2 && ySub < crystalsY; ++ySub)
                                {
                                    int dist = crystalPoints[xSub, ySub].Dist(i_3, y);
                                    if (dist < minDistance)
                                    {
                                        minDistance = dist;
                                        minPoint = crystalPoints[xSub, ySub];
                                    }
                                }
                            }

                            // Set the same color as the closest point color
                            for (xSub = 0; xSub < 3; xSub++)
                                imgStartPtr[y * stride + x + xSub] = avgColors[minPoint.x / size, minPoint.y / size, xSub];
                        }

                        if ((y & 63) == 0) reporter?.Report(h + y, 0, 2 * h - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
