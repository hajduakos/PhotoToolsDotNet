using System.Drawing;
using FilterLib.Blending;
using FilterLib.Filters.Adjustments;
using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Vintage filter.
    /// </summary>
    [Filter]
    public sealed class VintageFilter : FilterInPlaceBase
    {
        private int strength;

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
        /// Constructor.
        /// </summary>
        /// <param name="strength">Strength [0;100]</param>
        public VintageFilter(int strength = 0)
        {
            this.Strength = strength;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            using (Bitmap original = (Bitmap)image.Clone())
            {
                new ColorDodgeBlend(60).ApplyInPlace(image, original, new SubReporter(reporter, 0, 20, 0, 100));
                using (Bitmap sepia = new SepiaFilter().Apply(original, new SubReporter(reporter, 20, 40, 0, 100)))
                {
                    new MultiplyBlend(50).ApplyInPlace(image, sepia);
                }
                new ContrastFilter(15).ApplyInPlace(image, new SubReporter(reporter, 40, 60, 0, 100));
                new ColorHSLFilter(0, 5, 0).ApplyInPlace(image, new SubReporter(reporter, 60, 80, 0, 100));

                if (strength != 100)
                    new NormalBlend(100 - strength).ApplyInPlace(image, original, new SubReporter(reporter, 80, 100, 0, 100));
            }
            reporter?.Done();
        }
    }
}
