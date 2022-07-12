using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Screen blend mode.
    /// </summary>
    [Blend]
    public sealed class ScreenBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public ScreenBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
           return ((255 - compTop) * compBottom / 255f + compTop).ClampToByte();
        }
    }
}
