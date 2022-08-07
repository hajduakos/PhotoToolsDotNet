using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Util
{
    public static class Histogram
    {
        const int MAX_THREADS = 16;
        /// <summary>
        /// Get luminance histogram corresponding to an image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <returns>Luminance histogram (256 elements)</returns>
        public unsafe static int[] GetLuminanceHistogram(Image image)
        {
            // Divide into equal blocks for each thread (except the last which has the remainder)
            int threads = System.Math.Min(image.Height, MAX_THREADS);
            int blockSize = image.Height / threads;
            // Each thread has local data to avoid locking
            int[,] histogram = new int[threads, 256];
            Parallel.For(0, threads, t => { for (int i = 0; i < 256; ++i) histogram[t, i] = 0; });

            fixed (byte* start = image)
            {
                byte* start0 = start;
                Parallel.For(0, threads, t =>
                {
                    int yStart = t * blockSize;
                    int yEnd = t == threads - 1 ? image.Height : (t + 1) * blockSize;
                    byte* ptr = start0 + yStart * image.Width * 3;
                    for (int y = yStart; y < yEnd; ++y)
                    {
                        for (int x = 0; x < image.Width; ++x)
                        {
                            byte lum = (byte)RGB.GetLuminance(ptr[0], ptr[1], ptr[2]);
                            ++histogram[t, lum];
                            ptr += 3;
                        }
                    }
                });

            }
            // Finally merge the results from each thread
            int[] merged = new int[256];
            Parallel.For(0, 256, i =>
            {
                merged[i] = 0;
                for (int t = 0; t < threads; ++t) merged[i] += histogram[t, i];
            });
            return merged;
        }
    }
}
