﻿using FilterLib.Filters.Other;
using FilterLib.Reporting;
using FilterLib.Util;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Edges
{
    /// <summary>
    /// Base class for filters that perform two convolutions and combine them.
    /// </summary>
    public abstract class DualConvolutionBase : FilterInPlaceBase
    {
        private readonly ConvolutionFilter conv1, conv2;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conv1">First convolution</param>
        /// <param name="conv2">Second convolution</param>
        protected DualConvolutionBase(ConvolutionMatrix conv1, ConvolutionMatrix conv2)
        {
            this.conv1 = new(conv1);
            this.conv2 = new(conv2);
        }

        /// <summary>
        /// Combine two components after the convolutions.
        /// </summary>
        /// <param name="c1">First component</param>
        /// <param name="c2">Second component</param>
        /// <returns>Combination</returns>
        public virtual byte Combine(int c1, int c2) => System.MathF.Sqrt(c1 * c1 + c2 * c2).ClampToByte();

        /// <inheritdoc/>
        public override sealed unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            Image tmp = (Image)image.Clone();

            conv1.ApplyInPlace(tmp, reporter == null ? null : new SubReporter(reporter, 0, 33, 0, 100));
            conv2.ApplyInPlace(image, reporter == null ? null : new SubReporter(reporter, 34, 66, 0, 100));
            IReporter subRep = reporter == null ? null : new SubReporter(reporter, 67, 100, 0, 100);

            // Cache all different combinations
            byte[,] map = new byte[256, 256];
            Parallel.For(0, 256, x =>
            {
                for (int y = 0; y < 256; y++)
                    map[x, y] = Combine(x, y);
            });

            object reporterLock = new();
            int progress = 0;
            int width_3 = image.Width * 3;
            fixed (byte* imgStart = image, tmpStart = tmp)
            {
                byte* imgStart0 = imgStart;
                byte* tmpStart0 = tmpStart;
                Parallel.For(0, image.Height, y =>
                {
                    byte* imgPtr = imgStart0 + y * width_3;
                    byte* tmpPtr = tmpStart0 + y * width_3;
                    for (int x = 0; x < width_3; ++x)
                    {
                        *imgPtr = map[*imgPtr, *tmpPtr];
                        ++imgPtr;
                        ++tmpPtr;
                    }
                    if (subRep != null) lock (reporterLock) subRep.Report(++progress, 0, image.Height);
                });
            }
            reporter?.Done();
        }
    }
}
