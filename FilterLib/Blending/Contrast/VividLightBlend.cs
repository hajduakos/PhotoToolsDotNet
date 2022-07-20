using FilterLib.Util;

namespace FilterLib.Blending.Contrast
{
    /// <summary>
    /// Do a half-strength color burn blend for dark top pixels and a half-strength
    /// color dodge blend for light top pixels.
    /// </summary>
    [Blend]
    public sealed class VividLightBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public VividLightBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            float bf = compBottom / 255f;
            float tf = compTop / 255f;
            float blended;
            if (tf > .5f)
                blended = bf / (2f * (1 - tf));
            else
                blended = 1 - (1 - bf) / (2 * tf);
            return (blended * 255).ClampToByte();
        }
    }
}
