namespace FilterLib.Blending
{
    /// <summary>
    /// Base class for blend modes that process each (R/G/B) component individually.
    /// </summary>
    public abstract class PerComponentBlendBase : PerPixelBlendBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="opacity">Opacity[0;100]</param>
        protected PerComponentBlendBase(int opacity) : base(opacity) { }

        /// <summary>
        /// Blend a component (R/G/B) without considering opacity.
        /// </summary>
        /// <param name="compBottom">Bottom component</param>
        /// <param name="compTop">Top component</param>
        /// <returns>Blended value</returns>
        protected abstract byte BlendComponent(byte compBottom, byte compTop);

        /// <inheritdoc/>
        protected override sealed (byte, byte, byte) BlendPixel(byte botR, byte botG, byte botB, byte topR, byte topG, byte topB) =>
            (BlendComponent(botR, topR), BlendComponent(botG, topG), BlendComponent(botB, topB));
    }
}
