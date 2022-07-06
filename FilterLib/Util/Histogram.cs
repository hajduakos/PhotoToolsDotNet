namespace FilterLib.Util
{
    public static class Histogram
    {
        /// <summary>
        /// Get luminance histogram corresponding to an image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <returns>Luminance histogram (256 elements)</returns>
        public static int[] GetLuminanceHistogram(Image image)
        {
            // Array containing histogram values
            int[] histogram = new int[256];
            for (int i = 0; i < 256; ++i) histogram[i] = 0;
            unsafe
            {
                fixed (byte* start = image)
                {
                    int h = image.Height;
                    int width_3 = image.Width * 3;
                    unsafe
                    {
                        for (int y = 0; y < h; ++y)
                        {
                            byte* row = start + y * width_3;
                            for (int x = 0; x < width_3; x += 3)
                                ++histogram[(int)(.299 * row[x] + .587 * row[x + 1] + .114 * row[x + 2])];
                        }
                    }
                }
            }
            return histogram;
        }
    }
}
