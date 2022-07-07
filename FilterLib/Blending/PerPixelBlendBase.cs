using Math = System.Math;

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
        public override sealed unsafe void ApplyInPlace(Image bottom, Image top, Reporting.IReporter reporter = null)
        {
            reporter?.Start();
            BlendStart();
            fixed (byte* botStart = bottom, topStart = top)
            {
                int height = Math.Min(bottom.Height, top.Height);
                int width = Math.Min(bottom.Width, top.Width);

                for (int y = 0; y < height; ++y)
                {
                    byte* botPtr = botStart + (y * bottom.Width * 3);
                    byte* topPtr = topStart + (y * top.Width * 3);

                    for (int x = 0; x < width; ++x)
                    {
                        BlendPixel(botPtr, botPtr + 1, botPtr + 2, topPtr, topPtr + 1, topPtr + 2);
                        botPtr += 3;
                        topPtr += 3;
                    }

                    reporter?.Report(y, 0, height - 1);
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
