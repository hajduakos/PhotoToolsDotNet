using FilterLib.Util;

namespace FilterLib.Blending.Cancelation
{
    /// <summary>
    /// Divide bottom layer by top layer.
    /// </summary>
    [Blend]
    public sealed class DivideBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public DivideBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override byte BlendComponent(byte compBottom, byte compTop)
        {
            if (compTop == 0) return 255;
            return (compBottom / (float)compTop * 255f).ClampToByte();
        }
    }
}
