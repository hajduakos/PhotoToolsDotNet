using FilterLib.Util;

namespace FilterLib.Blending.Lighten
{
    /// <summary>
    /// Divide bottom layer by inverted top layer.
    /// </summary>
    [Blend]
    public sealed class ColorDodgeBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public ColorDodgeBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            if (compTop == 255) return 255;
            else return (compBottom / (255f - compTop) * 255f).ClampToByte();
        }
    }
}