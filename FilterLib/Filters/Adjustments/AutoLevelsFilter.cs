using FilterLib.Reporting;

namespace FilterLib.Filters.Adjustments
{
    [Filter("Automatic levels adjustment by histogram stretching.")]
    public sealed class AutoLevelsFilter : FilterInPlaceBase
    {
        private const float LocalLimitPct = 1f / 256;
        private const float GlobalLimitPct = 0.01f;

        /// <summary>
        /// Determine dark/light values to be used in levels filter by limiting the total/local number of
        /// pixels that are lost (become completely black or white).
        /// </summary>
        /// <returns>Tuple of dark and light cropping values</returns>
        private static (int, int) GetCroppingValues(Image image)
        {
            int[] lumHistogram = Util.Histogram.GetLuminanceHistogram(image);
            System.Diagnostics.Debug.Assert(lumHistogram.Length == 256, "Histogram length expected to be 256.");
            int totalPixels = image.Width * image.Height;
            // Local limit to avoid losing individual intensity levels
            float localLimit = totalPixels * LocalLimitPct;
            // Global limit to avoid losing too many total pixels
            float globalLimit = totalPixels * GlobalLimitPct;
            int light = 0, dark = 0;
            // Determine dark value
            int totalLost = 0;
            while (lumHistogram[dark] < localLimit && totalLost < globalLimit)
                totalLost += lumHistogram[dark++];
            // Determine light value
            totalLost = 0;
            while (lumHistogram[255 - light] < localLimit && totalLost < globalLimit && 255 - light - 1 > dark)
                totalLost += lumHistogram[255 - light++];
            return (dark, light);
        }

        /// <inheritdoc/>
        public override void ApplyInPlace(Image image, IReporter reporter = null)
        {
            reporter?.Start();
            (int dark, int light) = GetCroppingValues(image);
            reporter?.Report(50, 0, 100);
            new LevelsFilter(dark, 255 - light).ApplyInPlace(image, new SubReporter(reporter, 50, 100, 0, 100));
            reporter?.Done();
        }
    }
}
