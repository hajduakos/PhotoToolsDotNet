using FilterLib.Util;
using Bitmap = System.Drawing.Bitmap;
using Math = System.Math;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FilterLib.Blending
{
    /// <summary>
    /// Base class for blend modes that process each pixel individually.
    /// </summary>
    public abstract class PerPixelBlendBase : BlendInPlaceBase
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="opacity">Opacity[0;100]</param>
        protected PerPixelBlendBase(int opacity) : base(opacity) { }

        /// <inheritdoc/>
        public override sealed void ApplyInPlace(Image bottom, Image top, Reporting.IReporter reporter = null)
        {
            reporter?.Start();
            BlendStart();
            unsafe
            {
                fixed (byte* botStart = bottom, topStart = top)
                {
                    int height = Math.Min(bottom.Height, top.Height);
                    int width_3 = Math.Min(bottom.Width, top.Width) * 3;

                    for (int y = 0; y < height; ++y)
                    {
                        byte* botRow = botStart + (y * bottom.Width * 3);
                        byte* topRow = topStart + (y * top.Width * 3);

                        for (int x = 0; x < width_3; x += 3)
                            BlendPixel(
                                botRow + x, botRow + x + 1, botRow + x + 2,
                                topRow + x, topRow + x + 1, topRow + x + 2);

                        reporter?.Report(y, 0, height - 1);
                    }
                }
            }

            BlendEnd();
            reporter?.Done();
        }

        /// <summary>
        /// Gets called when processing starts.
        /// </summary>
        protected virtual void BlendStart() { }

        /// <summary>
        /// Blend an individual pixel.
        /// </summary>
        /// <param name="botR">Bottom red</param>
        /// <param name="botG">Bottom green</param>
        /// <param name="botB">Bottom blue</param>
        /// <param name="topR">Top red</param>
        /// <param name="topG">Top green</param>
        /// <param name="topB">Top blue</param>
        protected abstract unsafe void BlendPixel(byte* botR, byte* botG, byte* botB, byte* topR, byte* topG, byte* topB);

        /// <summary>
        /// Gets called when processing ends.
        /// </summary>
        protected virtual void BlendEnd() { }
    }
}
