namespace FilterLib.Util
{
    public static class Histogram
    {
        /// <summary>
        /// Get luminance histogram corresponding to an image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <returns>Luminance histogram (256 elements)</returns>
        public unsafe static int[] GetLuminanceHistogram(Image image)
        {
            // Array containing histogram values
            int[] histogram = new int[256];
            for (int i = 0; i < 256; ++i) histogram[i] = 0;

            fixed (byte* start = image)
            {
                byte* ptr = start;
                for (int y = 0; y < image.Height; ++y)
                {
                    for (int x = 0; x < image.Width; ++x)
                    {
                        ++histogram[(byte)RGB.GetLuminance(ptr[0], ptr[1], ptr[2])];
                        ptr += 3;
                    }
                }

            }
            return histogram;
        }
    }
}
