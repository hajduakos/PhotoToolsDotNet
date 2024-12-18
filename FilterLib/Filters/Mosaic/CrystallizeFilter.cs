﻿using FilterLib.Reporting;
using FilterLib.Util;
using Math = System.Math;
using Parallel = System.Threading.Tasks.Parallel;
using Random = System.Random;

namespace FilterLib.Filters.Mosaic
{
    /// <summary>
    /// Create a crystallized look using a variant of the Voronoi diagram.
    /// </summary>
    [Filter]
    public sealed class CrystallizeFilter : FilterInPlaceBase
    {
        private int size;
        private int averaging;

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
        private readonly struct Cpoint : System.IEquatable<Cpoint>
        {
            public readonly int x, y; // Coordinates

            // Constructor
            public Cpoint(int x = 0, int y = 0) { this.x = x; this.y = y; }

            // Get distance from other point (without square root)
            public int Dist(int x1, int y1) => (x - x1) * (x - x1) + (y - y1) * (y - y1);

            public bool Equals(Cpoint other) => x == other.x && y == other.y;

            public override bool Equals(object obj) => obj is Cpoint cpoint && Equals(cpoint);

            public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode();
        }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            // First step: divide the image into a grid with the given size
            // and generate one crystal point in each square
            // Additional points required if the width/height is not dividible by the size
            int crystalsX = image.Width / size + ((image.Width % size) == 0 ? 0 : 1);
            int crystalsY = image.Height / size + ((image.Height % size) == 0 ? 0 : 1);
            Cpoint[,] crystalPts = new Cpoint[crystalsX, crystalsY];
            RandomPool rndp = new(crystalsX, Seed);
            Parallel.For(0, crystalsX, xSub =>
            {
                Random rnd = rndp[xSub];
                for (int ySub = 0; ySub < crystalsY; ++ySub)
                {
                    int cx = Math.Min(xSub * size + rnd.Next() % size, image.Width - 1);
                    int cy = Math.Min(ySub * size + rnd.Next() % size, image.Height - 1);
                    crystalPts[xSub, ySub] = new Cpoint(cx, cy);
                }
            });

            int width_3 = image.Width * 3;
            int size_3 = size * 3;
            float avg = averaging / 100.0f;
            byte[,,] avgColors = new byte[crystalsX, crystalsY, 3];
            fixed (byte* start = image)
            {
                byte* start0 = start;
                int yMax = image.Height / size;
                if (yMax * size < image.Height) yMax++;
                // Second step: calculate avarege color of a square in the grid
                Parallel.For(0, yMax, y =>
                {
                    y *= size;
                    for (int x = 0; x < width_3; x += size_3)
                    {
                        float rSum = 0, gSum = 0, bSum = 0;
                        int n = 0;
                        for (int ySub = 0; ySub < size && y + ySub < image.Height; ++ySub)
                        {
                            for (int xSub = 0; xSub < size_3 && x + xSub < width_3; xSub += 3)
                            {
                                rSum += start0[(y + ySub) * width_3 + x + xSub];
                                gSum += start0[(y + ySub) * width_3 + x + xSub + 1];
                                bSum += start0[(y + ySub) * width_3 + x + xSub + 2];
                                ++n;
                            }
                        }
                        // Set average color as a combination of the average color and the representative point
                        Cpoint pnt = crystalPts[x / size_3, y / size];
                        avgColors[x / size_3, y / size, 0] = (byte)(avg * rSum / n + (1 - avg) * start0[pnt.y * width_3 + pnt.x * 3]);
                        avgColors[x / size_3, y / size, 1] = (byte)(avg * gSum / n + (1 - avg) * start0[pnt.y * width_3 + pnt.x * 3 + 1]);
                        avgColors[x / size_3, y / size, 2] = (byte)(avg * bSum / n + (1 - avg) * start0[pnt.y * width_3 + pnt.x * 3 + 2]);
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, yMax * 2);
                });

                // Third step: draw Voronoi diagram: for each pixel, find the closest crystal point
                // and use that as the color of the pixel
                progress = 0;
                Parallel.For(0, image.Height, y =>
                {
                    for (int x = 0; x < width_3; x += 3)
                    {
                        int x_div3 = x / 3;
                        int blockX = x_div3 / size;
                        int blockY = y / size;
                        // Optimization: closest point must belong to the surrounding 5x5 square of blocks
                        Cpoint minPoint = crystalPts[blockX, blockY];
                        int minDistance = minPoint.Dist(x_div3, y);
                        for (int xSub = Math.Max(0, blockX - 2); xSub <= blockX + 2 && xSub < crystalsX; ++xSub)
                        {
                            for (int ySub = Math.Max(0, blockY - 2); ySub <= blockY + 2 && ySub < crystalsY; ++ySub)
                            {
                                int dist = crystalPts[xSub, ySub].Dist(x_div3, y);
                                if (dist < minDistance)
                                {
                                    minDistance = dist;
                                    minPoint = crystalPts[xSub, ySub];
                                }
                            }
                        }

                        // Set the same color as the closest point color
                        for (int i = 0; i < 3; i++)
                            start0[y * width_3 + x + i] = avgColors[minPoint.x / size, minPoint.y / size, i];
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(image.Height + ++progress, 0, 2 * image.Height);
                });
            }
            reporter?.Done();
        }
    }
}
