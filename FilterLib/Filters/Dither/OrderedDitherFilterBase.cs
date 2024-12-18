﻿using FilterLib.Reporting;
using FilterLib.Util;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Filters.Dither
{
    /// <summary>
    /// Base class for ordered dithering filters.
    /// </summary>
    public abstract class OrderedDitherFilterBase : FilterInPlaceBase
    {
        /// <summary>
        /// Interface for ordered dither matrices.
        /// </summary>
        public interface IOrderedDitherMatrix
        {
            /// <summary>
            /// Get an element of the matrix.
            /// </summary>
            /// <param name="x">Column number</param>
            /// <param name="y">Row number</param>
            /// <returns>Element</returns>
            float this[int x, int y] { get; }

            /// <summary>
            /// Width of the matrix
            /// </summary>
            int Width { get; }

            /// <summary>
            /// Height of the matrix
            /// </summary>
            int Height { get; }
        }

        private int levels;

        /// <summary>
        /// Number of levels [2:256].
        /// </summary>
        [FilterParam]
        [FilterParamMin(2)]
        [FilterParamMax(256)]
        public int Levels
        {
            get { return levels; }
            set { levels = value.Clamp(2, 256); }
        }

        /// <summary>
        /// Dither matrix.
        /// </summary>
        protected IOrderedDitherMatrix Matrix { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levels">Number of levels [2;256]</param>
        /// <param name="matrix">Dither matrix</param>
        protected OrderedDitherFilterBase(int levels, IOrderedDitherMatrix matrix)
        {
            Levels = levels;
            Matrix = matrix;
        }

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            object reporterLock = new();
            int progress = 0;
            float intervalSize = 255f / (levels - 1);
            int width_3 = image.Width * 3;
            fixed (byte* start = image)
            {
                byte* start0 = start;
                Parallel.For(0, image.Height, y =>
                {
                    byte* ptr = start0 + y * width_3;
                    for (int x = 0; x < width_3; ++x)
                    {
                        // We need to round down or up to the nearest multiply of intervalSize.
                        // The precise treshold depends on the matrix, so that values inbetween are sometimes
                        // rounded down, and sometimes rounded up.
                        float roundedColor = System.MathF.Floor(*ptr / intervalSize) * intervalSize;
                        float treshold = roundedColor + Matrix[(x / 3) % Matrix.Width, y % Matrix.Height] * intervalSize;
                        *ptr = (treshold > *ptr ? roundedColor : (roundedColor + intervalSize)).ClampToByte();
                        ++ptr;
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, image.Height);
                });
            }
            reporter?.Done();
        }
    }
}
