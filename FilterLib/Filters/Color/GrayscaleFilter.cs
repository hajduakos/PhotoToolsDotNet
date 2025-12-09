using FilterLib.Util;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Convert to grayscale using custom weights for the different components.
    /// </summary>
    [Filter]
    public sealed class GrayscaleFilter : PerPixelFilterBase
    {
        /// <summary>
        /// Red ratio [0;100]
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(100)]
        public int Red
        {
            get;
            set { field = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Green ratio [0;100]
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(100)]
        public int Green
        {
            get;
            set { field = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Blue ratio [0;100]
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(100)]
        public int Blue
        {
            get;
            set { field = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="red">Red ratio [0;100]</param>
        /// <param name="green">Green ratio [0;100]</param>
        /// <param name="blue">Blue ratio [0;100]</param>
        public GrayscaleFilter(int red = 30, int green = 59, int blue = 11)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        private float redF, greenF, blueF;

        /// <inheritdoc/>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            redF = Red / 100f;
            greenF = Green / 100f;
            blueF = Blue / 100f;
        }

        /// <inheritdoc/>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            *r = *g = *b = (redF * (*r) + greenF * (*g) + blueF * (*b)).ClampToByte();
        }
    }
}
