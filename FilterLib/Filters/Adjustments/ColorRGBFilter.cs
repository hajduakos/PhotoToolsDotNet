using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Adjust color in red-green-blue (RGB) color space by increasing/decreasing the value
    /// of each component by a fixed amount.
    /// </summary>
    [Filter]
    public sealed class ColorRGBFilter : PerPixelFilterBase
    {
        private int red, green, blue;

        /// <summary>
        /// Adjustment to red component [-255;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(-255)]
        [FilterParamMax(255)]
        public int Red
        {
            get { return red; }
            set { red = value.Clamp(-255, 255); }
        }

        /// <summary>
        /// Adjustment to green component [-255;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(-255)]
        [FilterParamMax(255)]
        public int Green
        {
            get { return green; }
            set { green = value.Clamp(-255, 255); }
        }

        /// <summary>
        /// Adjustment to blue component [-255;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(-255)]
        [FilterParamMax(255)]
        public int Blue
        {
            get { return blue; }
            set { blue = value.Clamp(-255, 255); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="red">Adjustment to red component [-255;255]</param>
        /// <param name="green">Adjustment to green component [-255;255]</param>
        /// <param name="blue">Adjustment to blue component [-255;255]</param>
        public ColorRGBFilter(int red = 0, int green = 0, int blue = 0)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        // Caches
        private byte[] redMap = null;
        private byte[] greenMap = null;
        private byte[] blueMap = null;

        /// <inheritdoc/>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            // Pre-calculate and cache all possibilities
            redMap = new byte[256];
            greenMap = new byte[256];
            blueMap = new byte[256];
            for (int x = 0; x < 256; ++x)
            {
                redMap[x] = (x + red).ClampToByte();
                greenMap[x] = (x + green).ClampToByte();
                blueMap[x] = (x + blue).ClampToByte();
            }
        }

        /// <inheritdoc/>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            System.Diagnostics.Debug.Assert(redMap != null);
            System.Diagnostics.Debug.Assert(greenMap != null);
            System.Diagnostics.Debug.Assert(blueMap != null);
            // Just use cache
            *r = redMap[*r];
            *g = greenMap[*g];
            *b = blueMap[*b];
        }

        /// <inheritdoc/>
        protected override void ApplyEnd()
        {
            redMap = greenMap = blueMap = null;
            base.ApplyEnd();
        }
    }
}
