using FilterLib.Util;

namespace FilterLib.Blending.Contrast
{
    /// <summary>
    /// Soft light blend based on PS formula.
    /// </summary>
    [Blend]
    public sealed class SoftLightBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public SoftLightBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            if (compTop <= 127)
                return (2 * compBottom * compTop / 255f + compBottom / 255f * compBottom / 255f * (255 - 2 * compTop)).ClampToByte();
            else
                return (2 * compBottom / 255f * (255 - compTop) + System.MathF.Sqrt(compBottom / 255f) * (2 * compTop - 255)).ClampToByte();
        }
    }
}
