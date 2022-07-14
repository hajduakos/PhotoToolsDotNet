using FilterLib.Util;

namespace FilterLib.Blending.Contrast
{
    /// <summary>
    /// Do a half-strength multiply blend for dark bottom pixels and a half-strength
    /// screen blend for light bottom pixels.
    /// </summary>
    [Blend]
    public sealed class OverlayBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public OverlayBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            if (compBottom < 127)
                return (2 * compBottom * compTop / 255f).ClampToByte();
            else
                return (255 - 2 * (255 - compTop) * (255 - compBottom) / 255f).ClampToByte();
        }
    }
}
