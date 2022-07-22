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
        protected override byte BlendComponent(byte compBottom, byte compTop) =>
            (compBottom + 2 * compTop - 255).ClampToByte();
    }
}
