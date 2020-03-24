namespace FilterLib.Blending
{
    /// <summary>
    /// Base class for blend modes that process each (R/G/B) component individually.
    /// </summary>
    public abstract class PerComponentBlendBase : PerPixelBlendBase
    {
        /// <summary>
        /// Blend a component (R/G/B).
        /// </summary>
        /// <param name="compBottom">Bottom component</param>
        /// <param name="compTop">Top component</param>
        protected abstract unsafe void BlendComponent(byte* compBottom, byte* compTop);

        /// <summary>
        /// Blend an individual pixel.
        /// </summary>
        /// <param name="botR">Bottom red</param>
        /// <param name="botG">Bottom green</param>
        /// <param name="botB">Bottom blue</param>
        /// <param name="topR">Top red</param>
        /// <param name="topG">Top green</param>
        /// <param name="topB">Top blue</param>
        protected override sealed unsafe void BlendPixel(byte* botR, byte* botG, byte* botB, byte* topR, byte* topG, byte* topB)
        {
            BlendComponent(botR, topR);
            BlendComponent(botG, topG);
            BlendComponent(botB, topB);
        }
    }
}
