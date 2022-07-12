using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Multiply blend mode.
    /// </summary>
    [Blend]
    public sealed class MultiplyBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public MultiplyBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            return (compBottom * compTop / 255f).ClampToByte();
        }
    }
}
