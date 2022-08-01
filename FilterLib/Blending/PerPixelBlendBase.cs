using FilterLib.Util;
using Math = System.Math;
using Parallel = System.Threading.Tasks.Parallel;

namespace FilterLib.Blending
{
    /// <summary>
    /// Base class for blend modes that process each pixel individually
    /// and applies the result in the bottom image with the given opacity.
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
            object reporterLock = new();
            int progress = 0;
            BlendStart();
            float op1 = Opacity / 100.0f;
            float op0 = 1 - op1;
            fixed (byte* botStart = bottom, topStart = top)
            {
                byte* botStart0 = botStart;
                byte* topStart0 = topStart;
                int height = Math.Min(bottom.Height, top.Height);
                int width = Math.Min(bottom.Width, top.Width);

                Parallel.For(0, height, y =>
                {
                    byte* botPtr = botStart0 + y * bottom.Width * 3;
                    byte* topPtr = topStart0 + y * top.Width * 3;

                    for (int x = 0; x < width; ++x)
                    {
                        (byte r, byte g, byte b) = BlendPixel(botPtr[0], botPtr[1], botPtr[2], topPtr[0], topPtr[1], topPtr[2]);
                        botPtr[0] = (op0 * botPtr[0] + op1 * r).ClampToByte();
                        botPtr[1] = (op0 * botPtr[1] + op1 * g).ClampToByte();
                        botPtr[2] = (op0 * botPtr[2] + op1 * b).ClampToByte();
                        botPtr += 3;
                        topPtr += 3;
                    }
                    if (reporter != null) lock (reporterLock) reporter.Report(++progress, 0, height);
                });
            }
            BlendEnd();
            reporter?.Done();
        }

        /// <summary>
        /// Gets called when processing starts.
        /// </summary>
        protected virtual void BlendStart() { }

        /// <summary>
        /// Blend an individual pixel without considering opacity.
        /// </summary>
        /// <param name="botR">Bottom red</param>
        /// <param name="botG">Bottom green</param>
        /// <param name="botB">Bottom blue</param>
        /// <param name="topR">Top red</param>
        /// <param name="topG">Top green</param>
        /// <param name="topB">Top blue</param>
        /// <returns>Blended value (R, G, B)</returns>
        protected abstract (byte, byte, byte) BlendPixel(byte botR, byte botG, byte botB, byte topR, byte topG, byte topB);

        /// <summary>
        /// Gets called when processing ends.
        /// </summary>
        protected virtual void BlendEnd() { }
    }
}
