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
            float bf = compBottom / 255f;
            float tf = compTop / 255f;
            float blended;
            if (tf < .5f)
                blended = 2 * bf * tf + bf * bf * (1 - 2 * tf);
            else
                blended = 2 * bf * (1 - tf) + System.MathF.Sqrt(bf) * (2 * tf - 1);
            return (blended * 255).ClampToByte();
        }
    }
}
