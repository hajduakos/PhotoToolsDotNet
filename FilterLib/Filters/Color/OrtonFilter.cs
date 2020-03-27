using System.Drawing;
using FilterLib.Blending;
using FilterLib.Filters.Blur;
using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Orton filter.
    /// </summary>
    [Filter]
    public sealed class OrtonFilter : FilterInPlaceBase
    {
        private int strength, radius;

        /// <summary>
        /// Strength [0;100].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        [FilterParamMax(100)]
        public int Strength
        {
            get { return strength; }
            set { strength = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Blur radius [0;...].
        /// </summary>
        [FilterParam]
        [FilterParamMin(0)]
        public int Radius
        {
            get { return radius; }
            set { radius = System.Math.Max(0, value); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="strength">Strength [0;100]</param>
        /// <param name="radius">Blur radius [0;...]</param>
        public OrtonFilter(int strength = 0, int radius = 0)
        {
            this.Strength = strength;
            this.Radius = radius;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            using Bitmap screened = new ScreenBlend(100).Apply(image, (Bitmap)image.Clone());
            using Bitmap blurred = new GaussianBlurFilter(radius).Apply(screened, reporter);
            using Bitmap multiplied = new MultiplyBlend(100).Apply(screened, blurred);
            new NormalBlend(strength).ApplyInPlace(image, multiplied);
        }
    }
}
