using FilterLib.Filters.Other;
using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Edges
{
    /// <summary>
    /// Prewitt filter combining two convolutions.
    /// </summary>
    [Filter]
    public sealed class PrewittFilter : FilterInPlaceBase
    {
        private readonly ConvolutionFilter conv1 =
            new(new Conv3x3(-1, -1, -1, 0, 0, 0, 1, 1, 1));

        private readonly ConvolutionFilter conv2 =
            new(new Conv3x3(-1, 0, 1, -1, 0, 1, -1, 0, 1));

        /// <inheritdoc/>
        public override unsafe void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image
            Image tmp = (Image)image.Clone();

            // Apply vertical convolution
            conv1.ApplyInPlace(tmp, new SubReporter(reporter, 0, 33, 0, 100));
            // Apply horizontal convolution
            conv2.ApplyInPlace(image, new SubReporter(reporter, 34, 66, 0, 100));
            IReporter subRep = new SubReporter(reporter, 67, 100, 0, 100);

            // Cache all different combinations
            byte[,] map = new byte[256, 256];
            for (int x = 0; x < 256; x++)
                for (int y = 0; y < 256; y++)
                    map[x, y] = System.MathF.Sqrt(x * x + y * y).ClampToByte();


            int width_3 = image.Width * 3;
            fixed (byte* imgStart = image, tmpStart = tmp)
            {
                byte* imgPtr = imgStart;
                byte* tmpPtr = tmpStart;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < width_3; ++x)
                    {
                        *imgPtr = map[*imgPtr, *tmpPtr];
                        ++imgPtr;
                        ++tmpPtr;
                    }
                    subRep.Report(y, 0, image.Height - 1);
                }

            }
            reporter?.Done();
        }
    }
}
