using System.Drawing;
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
        /// <param name="dark">Dark cropping value (out parameter)</param>
        /// <param name="light">Light cropping value (out parameter)</param>
        public static void GetCroppingValues(Bitmap image, out int dark, out int light)
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
            // Calculate light crop lvalue like the dark
            while (hist[255 - cropLight] < localLimit && total < totalLimit) total += hist[255 - cropLight++];
            // Set out parameters
            dark = cropDark; light = cropLight;
        }

        /// <summary>
        /// Apply filter by modifying the original image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override void ApplyInPlace(Bitmap image, IReporter reporter = null)
        {
            if (reporter != null) reporter.Start();
            // Calculate cropping values
            GetCroppingValues(image, out int cropDark, out int cropLight);
            if (reporter != null) reporter.Report(50, 0, 100);
            new LevelsFilter(cropDark, 255 - cropLight).ApplyInPlace(image, new SubReporter(reporter, 50, 100, 0, 100));
            if (reporter != null) reporter.Done();
        }
    }
}
