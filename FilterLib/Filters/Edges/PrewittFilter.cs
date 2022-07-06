using FilterLib.Filters.Other;
using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Edges
{
    /// <summary>
    /// Prewitt filter.
    /// </summary>
    [Filter]
    public sealed class PrewittFilter : FilterInPlaceBase
    {
        private readonly ConvolutionFilter conv1 =
            new(new Conv3x3(-1, -1, -1, 0, 0, 0, 1, 1, 1));

        private readonly ConvolutionFilter conv2 =
            new(new Conv3x3(-1, 0, 1, -1, 0, 1, -1, 0, 1));

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image
            Image tmp = (Image)image.Clone();

            // Apply vertical convolution
            conv1.ApplyInPlace(tmp, new SubReporter(reporter, 0, 33, 0, 100));
            // Apply horizontal convolution
            conv2.ApplyInPlace(image, new SubReporter(reporter, 34, 66, 0, 100));
            IReporter subRep = new SubReporter(reporter, 67, 100, 0, 100);

            // Lock bits
            int width_3 = image.Width * 3;
            int h = image.Height;
            int x, y, nVal;
            // Calculate map
            byte[,] map = new byte[256, 256];
            for (x = 0; x < 256; x++)
            {
                for (y = 0; y < 256; y++)
                {
                    // Calculate new value
                    nVal = (int)System.MathF.Sqrt(x * x + y * y);
                    // Correction
                    if (nVal > 255) nVal = 255;
                    // Overwrite original
                    map[x, y] = (byte)nVal;
                }
            }

            unsafe
            {
                fixed (byte* start = image, tmpStart = tmp)
                {
                    // Iterate through rows
                    for (y = 1; y < h - 1; ++y)
                    {
                        // Get rows
                        byte* row = start + (y * width_3);
                        byte* rowTmp = tmpStart + (y * width_3);
                        // Iterate through columns
                        for (x = 3; x < width_3 - 3; ++x)
                        {
                            // Overwrite original
                            row[x] = map[row[x], rowTmp[x]];
                        }
                        subRep.Report(y, 0, h - 1);
                    }

                }
            }

            reporter?.Done();
        }
    }
}
