using FilterLib.Util;

namespace FilterLib.Filters.Adjustments
{
    [Filter("Adjust the red, green and blue values of each pixel by a fixed amount.")]
    public sealed class ColorRGBFilter : PerPixelFilterBase
    {
        /// <summary>
        /// Adjustment to red component [-255;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(-255)]
        [FilterParamMax(255)]
        public int Red
        {
            get;
            set { field = value.Clamp(-255, 255); }
        }

        /// <summary>
        /// Adjustment to green component [-255;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(-255)]
        [FilterParamMax(255)]
        public int Green
        {
            get;
            set { field = value.Clamp(-255, 255); }
        }

        /// <summary>
        /// Adjustment to blue component [-255;255].
        /// </summary>
        [FilterParam]
        [FilterParamMin(-255)]
        [FilterParamMax(255)]
        public int Blue
        {
            get;
            set { field = value.Clamp(-255, 255); }
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
        private byte[] redMap;
        private byte[] greenMap;
        private byte[] blueMap;

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
                redMap[x] = (x + Red).ClampToByte();
                greenMap[x] = (x + Green).ClampToByte();
                blueMap[x] = (x + Blue).ClampToByte();
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
