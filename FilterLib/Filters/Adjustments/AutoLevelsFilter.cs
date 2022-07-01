using FilterLib.Reporting;
using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;

namespace FilterLib.Filters.Adjustments
{
    /// <summary>
    /// Automatic levels adjustment filter.
    /// </summary>
    [Filter]
    public sealed class AutoLevelsFilter : FilterInPlaceBase
    {
        /// <summary>
        /// Get automatic level cropping values.
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns>A tuple of the dark and light cropping values</returns>
        public static (int, int) GetCroppingValues(Bitmap image)
        {
            // Get luminance histogram
            int[] hist = Histogram.GetLuminanceHistogram(image);
            // Total pixel count
            int pixels = image.Width * image.Height;
            // Crop limits
            int localLimit = pixels / 256;
            int totalLimit = pixels / 100;
            // Dark and light crop values
            int cropLight = 0, cropDark = 0;
            // Total count of cropped pixels
            int total = 0;
            // Dark crop value increases until we reach the crop limit, or the total number
            // of cropped pixels reaches 1%
            while (hist[cropDark] < localLimit && total < totalLimit) total += hist[cropDark++];
            total = 0;
            // Calculate light crop value like the dark
            while (hist[255 - cropLight] < localLimit && total < totalLimit) total += hist[255 - cropLight++];
            return (cropDark, cropLight);
        }

        /// <inheritdoc/>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            reporter?.Start();
            // Calculate cropping values
            (int cropDark, int cropLight) = GetCroppingValues(image);
            reporter?.Report(50, 0, 100);
            new LevelsFilter(cropDark, 255 - cropLight).ApplyInPlace(image, new SubReporter(reporter, 50, 100, 0, 100));
            reporter?.Done();
        }
    }
}
