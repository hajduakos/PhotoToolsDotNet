using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Color burn blend mode.
    /// </summary>
    [Blend]
    public sealed class ColorBurnBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public ColorBurnBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            if (compTop == 0) return 0;
            else return (255 - (255 - compBottom) / (float)compTop * 255f).ClampToByte();
        }
    }
}
