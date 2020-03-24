using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Normal blend mode.
    /// </summary>
    public sealed class NormalBlend : PerComponentBlendBase
    {
        private int opacity;

        /// <summary> Opacity [0:100] </summary>
        public int Opacity
        {
            get { return opacity; }
            set { opacity = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public NormalBlend(int opacity)
        {
            Opacity = opacity;
        }

        float op0, op1;

        /// <summary>
        /// Gets called when processing starts.
        /// </summary>
        protected override void BlendStart()
        {
            op1 = opacity / 100.0f;
            op0 = 1 - op1;
        }

        /// <summary>
        /// Blend a component (R/G/B).
        /// </summary>
        /// <param name="compBottom">Bottom component</param>
        /// <param name="compTop">Top component</param>
        protected override unsafe void BlendComponent(byte* compBottom, byte* compTop)
        {
            compBottom[0] = (byte)(op0 * compBottom[0] + op1 * compTop[0]);
        }
    }
}
