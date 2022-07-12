using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Linear burn blend mode.
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
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            return (compBottom + compTop - 255).ClampToByte();
        }
    }
}
