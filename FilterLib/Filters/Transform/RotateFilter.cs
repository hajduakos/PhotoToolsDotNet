using FilterLib.Reporting;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FilterLib.Filters.Transform
{
    /// <summary>
    /// Rotate filter.
    /// </summary>
    [Filter]
    public sealed class RotateFilter : FilterBase
    {
        /// <summary>
        /// Rotation angle [0;360].
        /// </summary>
        [FilterParam]
        [FilterParamMinF(0)]
        [FilterParamMaxF(360)]
        public float Angle { get; set; }

        /// <summary>
        /// Crop the image to fit.
        /// </summary>
        [FilterParam]
        public bool Crop { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="angle">Rotation angle [0;360]</param>
        /// <param name="crop">Crop the image to fit</param>
        public RotateFilter(float angle = 0, bool crop = false)
        {
            Angle = angle;
            Crop = crop;
        }

        /// <inheritdoc/>
        public override Bitmap Apply(Bitmap image, IReporter reporter = null)
        {
            if (MathF.Abs(Angle) < 0.001f) return (Bitmap)image.Clone();

            reporter?.Start();
            float angRad = Angle / 180 * MathF.PI;
            float sinAng = MathF.Sin(angRad);
            float cosAng = MathF.Cos(angRad);
            Bitmap rotated;
            if (Crop)
            {
                int newHeight = (int)(image.Height / (image.Width / (float)image.Height * Math.Abs(sinAng) + Math.Abs(cosAng)));
                rotated = new Bitmap((int)(image.Width / (float)image.Height * newHeight), newHeight);
            }
            else
            {
                rotated = new Bitmap(
                    (int)(Math.Abs(sinAng) * image.Height + Math.Abs(cosAng * image.Width)),
                    (int)(Math.Abs(cosAng) * image.Height + Math.Abs(sinAng * image.Width)));
            }

            using (Graphics gfx = Graphics.FromImage(rotated))
            {
                gfx.SmoothingMode = SmoothingMode.HighQuality;
                gfx.InterpolationMode = InterpolationMode.High;
                gfx.Clear(System.Drawing.Color.White);

                System.Drawing.Drawing2D.Matrix m = gfx.Transform;
                m.RotateAt(Angle, new PointF(rotated.Width / 2f, rotated.Height / 2f), MatrixOrder.Append);
                gfx.Transform = m;

                gfx.DrawImage(image, new PointF((rotated.Width - image.Width) / 2f, (rotated.Height - image.Height) / 2f));
            }
            reporter?.Done();
            return rotated;
        }
    }
}
