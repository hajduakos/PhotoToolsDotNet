using FilterLib.Reporting;
using Math = System.Math;
using MathF = System.MathF;

namespace FilterLib.Filters.Blur
{
    /// <summary>
    /// Motion blur filter.
    /// </summary>
    [Filter]
    public sealed class MotionBlurFilter : FilterInPlaceBase
    {
        private int length;

        /// <summary>
        /// Blur length [0;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int Length
        {
            get { return length; }
            set { length = Math.Max(0, value); }
        }

        /// <summary>
        /// Blur angle in degrees [0;360].
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        [FilterParamMaxF(360)]
        public float Angle { get; set; }

        /// <summary>
        /// Constructor with blur lenght and angle
        /// </summary>
        /// <param name="length">Blur length</param>
        /// <param name="angle">Blur angle in degrees</param>
        public MotionBlurFilter(int length = 0, float angle = 0f)
        {
            Angle = angle;
            Length = length;
        }

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            // Clone image (the clone won't be modified)
            Image original = (Image)image.Clone();
            unsafe
            {
                fixed (byte* start = image, origStart = original)
                {
                    int width_3 = image.Width * 3;
                    float sinAngle = MathF.Sin(Angle * MathF.PI / 180);
                    float cosAngle = MathF.Cos(Angle * MathF.PI / 180);
                    int idx, rSum, gSum, bSum, n;
                    // Iterate through rows
                    for (int y = 0; y < image.Height; ++y)
                    {
                        // Iterate through columns
                        for (int x = 0; x < width_3; x += 3)
                        {
                            // We have to sum each component through a line
                            bSum = gSum = rSum = n = 0;
                            // Iterate through each pixel of the line
                            for (int k = -length; k <= length; ++k)
                            {
                                int dx = (int)MathF.Round(k * cosAngle); // Horizontal distance from the center pixel
                                int dy = (int)MathF.Round(k * sinAngle); // Vertical distance from the center pixel
                                int xAct = x / 3 + dx; // x coord. of the actual pixel
                                int yAct = y + dy; // y coord. of the actual pixel
                                // If the pixel is in the bounds of the image
                                if (xAct >= 0 && xAct < image.Width && yAct >= 0 && yAct < image.Height)
                                {
                                    idx = yAct * width_3 + xAct * 3;   // Calculate index
                                    rSum += origStart[idx + 2]; // Sum red component
                                    gSum += origStart[idx + 1]; // Sum blue component
                                    bSum += origStart[idx];     // Sum green component
                                    ++n; // Number of items
                                }
                            }
                            idx = y * width_3 + x; // Index of the center pixel
                            // Calculate average
                            start[idx + 2] = (byte)(rSum / n);
                            start[idx + 1] = (byte)(gSum / n);
                            start[idx] = (byte)(bSum / n);
                        }
                        reporter?.Report(y, 0, image.Height - 1);
                    }
                }
            }
            reporter?.Done();
        }
    }
}
