using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Do a half-strength multiply blend for dark top pixels and a half-strength
    /// screen blend for light top pixels.
    /// </summary>
    [Blend]
    public sealed class HardLightBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public HardLightBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            if (compTop < 127)
                return (2 * compBottom * compTop / 255f).ClampToByte();
            else
                return (255 - 2 * (255 - compTop) * (255 - compBottom) / 255f).ClampToByte();
        }
    }
}
