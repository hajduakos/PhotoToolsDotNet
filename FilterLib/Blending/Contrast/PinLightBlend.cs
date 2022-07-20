using FilterLib.Util;

namespace FilterLib.Blending.Contrast
{
    /// <summary>
    /// Do a half-strength darken blend for dark top pixels and a half-strength
    /// lighten blend for light top pixels.
    /// </summary>
    [Blend]
    public sealed class PinLightBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public PinLightBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            float bf = compBottom / 255f;
            float tf = compTop / 255f;
            float blended;
            if (tf > 0.5) blended = System.MathF.Max(bf, 2 * tf - 1);
            else blended = System.MathF.Min(bf, 2 * tf);
            return (blended * 255).ClampToByte();
        }
    }
}
