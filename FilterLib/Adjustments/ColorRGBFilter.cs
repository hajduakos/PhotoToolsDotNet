using FilterLib.Util;

namespace FilterLib.Adjustments
{
    /// <summary>
    /// Color adjustment in red-green-blue color space.
    /// </summary>
    public sealed class ColorRGBFilter : PerPixelFilterBase
    {
        private int red, green, blue;

        /// <summary>
        /// Red value [-255;255].
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
        /// Green value [-255;255].
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
        /// Blue value [-255;255].
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
        /// Constructor with R,G,B parameters.
        /// </summary>
        /// <param name="red">Red value [-255;255]</param>
        /// <param name="green">Green value [-255;255]</param>
        /// <param name="blue">Blue value [-255;255]</param>
        public ColorRGBFilter(int red = 0, int green = 0, int blue = 0)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        byte[] redMap = null;
        byte[] greenMap = null;
        byte[] blueMap = null;

        /// <summary>
        /// Gets called when filter starts applying.
        /// </summary>
        protected override void ApplyStart()
        {
            base.ApplyStart();
            redMap = new byte[256];
            greenMap = new byte[256];
            blueMap = new byte[256];
            for (int x = 0; x < 256; ++x)
            {
                redMap[x] = (byte)(x + red).Clamp(0, 255);
                greenMap[x] = (byte)(x + green).Clamp(0, 255);
                blueMap[x] = (byte)(x + blue).Clamp(0, 255);
            }
        }

        /// <summary>
        /// Gets called for each pixel independently.
        /// </summary>
        /// <param name="r">Pointer to red value</param>
        /// <param name="g">Pointer to green value</param>
        /// <param name="b">Pointer to blue value</param>
        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            *r = redMap[*r];
            *g = greenMap[*g];
            *b = blueMap[*b];
        }

        /// <summary>
        /// Gets called when filter finishes applying.
        /// </summary>
        protected override void ApplyEnd()
        {
            redMap = greenMap = blueMap = null;
            base.ApplyEnd();
        }
    }
}
