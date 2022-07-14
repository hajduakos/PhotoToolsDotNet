using FilterLib.Blending.Darken;
using FilterLib.Blending.Lighten;
using FilterLib.Blending.Normal;
using FilterLib.Filters.Blur;
using FilterLib.Reporting;
using FilterLib.Util;

namespace FilterLib.Filters.Color
{
    /// <summary>
    /// Orton filter, creating a soft glow look.
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
            Strength = strength;
            Radius = radius;
        }

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            Image screened = new ScreenBlend(100).Apply(image, image, new SubReporter(reporter, 0, 25, 0, 100));
            Image blurred = new GaussianBlurFilter(radius).Apply(screened, new SubReporter(reporter, 25, 50, 0, 100));
            Image multiplied = new MultiplyBlend(100).Apply(screened, blurred, new SubReporter(reporter, 50, 75, 0, 100));
            new NormalBlend(strength).ApplyInPlace(image, multiplied, new SubReporter(reporter, 75, 100, 0, 100));
            reporter?.Done();
        }
    }
}
