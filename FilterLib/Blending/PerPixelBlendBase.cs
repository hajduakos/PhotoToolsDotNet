using FilterLib.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;

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

        /// <summary>
        /// Blend two images together, the result is in the bottom image.
        /// </summary>
        /// <param name="bottom">Bottom image (will be changed)</param>
        /// <param name="top">Top image</param>
        /// <param name="reporter">Reporter (optional)</param>
        public override sealed void ApplyInPlace(Bitmap bottom, Bitmap top, Reporting.IReporter reporter = null)
        {
            reporter?.Start();
            BlendStart();

            // Lock bits
            using (DisposableBitmapData bmdBot = new(bottom, PixelFormat.Format24bppRgb))
            using (DisposableBitmapData bmdTop = new(top, PixelFormat.Format24bppRgb))
            {
                int h = Math.Min(bottom.Height, top.Height);
                int wMul3 = Math.Min(bottom.Width, top.Width) * 3;
                int botStride = bmdBot.Stride, topStride = bmdTop.Stride;
                int x, y;
                unsafe
                {
                    // Iterate through rows
                    for (y = 0; y < h; ++y)
                    {
                        // Get row
                        byte* botRow = (byte*)bmdBot.Scan0 + (y * botStride);
                        byte* topRow = (byte*)bmdTop.Scan0 + (y * topStride);
                        // Iterate through columns
                        for (x = 0; x < wMul3; x += 3)
                            BlendPixel(
                                botRow + x + 2, botRow + x + 1, botRow + x,
                                topRow + x + 2, topRow + x + 1, topRow + x);

                        if ((y & 63) == 0) reporter?.Report(y, 0, h - 1);
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
