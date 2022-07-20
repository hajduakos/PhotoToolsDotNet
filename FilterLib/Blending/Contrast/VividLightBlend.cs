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
            if (compTop > 127)
                return (compBottom / 2f / (1 - compTop / 255f)).ClampToByte();
            else
                return (255 - (255 - compBottom) / (2 * compTop / 255f)).ClampToByte();
        }
    }
}
