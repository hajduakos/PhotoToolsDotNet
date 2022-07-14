using FilterLib.Util;

namespace FilterLib.Blending.Inversion
{
    /// <summary>
    /// Calculate the (Absolute value) of the difference of the two layers.
    /// </summary>
    [Blend]
    public sealed class DifferenceBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public DifferenceBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop) =>
            System.Math.Abs(compBottom - compTop).ClampToByte();
    }
}
