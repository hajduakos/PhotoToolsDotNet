﻿namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Bayer dither is an ordered dither with a specific recursively defined matrix.
    /// </summary>
    [Filter]
    public class BayerDitherFilter : OrderedDitherFilterBase
    {
        private int size;

        /// <summary>
        /// Size of the matrix [1;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(1)]
        public int Size
        {
            get { return size; }
            set
            {
                size = System.Math.Max(1, value);
                Matrix = new BayerDitherMatrix(size);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2:256]</param>
        /// <param name="size">Size of the matrix [1;...]</param>
        public BayerDitherFilter(int levels = 256, int size = 1)
            : base(levels, new BayerDitherMatrix(size)) => Size = size;
    }
}
