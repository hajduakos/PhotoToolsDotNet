using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Add bottom and top layer.
    /// </summary>
    [Blend]
    public sealed class LinearDodgeBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public LinearDodgeBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop) =>
            (compBottom + compTop).ClampToByte();
    }
}
