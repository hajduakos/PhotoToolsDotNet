using FilterLib.Util;

namespace FilterLib.Blending.Contrast
{
    /// <summary>
    /// Do a linear burn blend with twice the effect of the top pixel.
    /// </summary>
    [Blend]
    public sealed class LinearLightBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public LinearLightBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            float bf = compBottom / 255f;
            float tf = compTop / 255f;
            float blended = bf + 2 * tf - 1;
            return (blended * 255).ClampToByte();
        }
    }
}
