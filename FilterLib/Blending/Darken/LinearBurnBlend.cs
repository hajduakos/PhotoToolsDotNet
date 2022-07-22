using FilterLib.Util;

namespace FilterLib.Blending.Darken
{
    /// <summary>
    /// Add bottom and top layer, then subtract white.
    /// </summary>
    [Blend]
    public sealed class LinearBurnBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public LinearBurnBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override byte BlendComponent(byte compBottom, byte compTop) =>
            (compBottom + compTop - 255).ClampToByte();
    }
}
