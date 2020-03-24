namespace FilterLib.Blending
{
    /// <summary>
    /// Lighten blend mode.
    /// </summary>
    public sealed class LightenBlend : PerPixelBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public LightenBlend(int opacity = 100) : base(opacity) { }

        float op0, op1;

        /// <summary>
        /// Gets called when processing starts.
        /// </summary>
        protected override void BlendStart()
        {
            op1 = Opacity / 100.0f;
            op0 = 1 - op1;
        }

        /// <summary>
        /// Blend an individual pixel.
        /// </summary>
        /// <param name="botR">Bottom red</param>
        /// <param name="botG">Bottom green</param>
        /// <param name="botB">Bottom blue</param>
        /// <param name="topR">Top red</param>
        /// <param name="topG">Top green</param>
        /// <param name="topB">Top blue</param>
        protected override unsafe void BlendPixel(byte* botR, byte* botG, byte* botB, byte* topR, byte* topG, byte* topB)
        {
            // Calculate luminance: 0.299*R + 0.587*G + 0.114*B
            float lum1 = .299f * (*botR) + .587f * (*botG) + .114f * (*botB);
            float lum2 = .299f * (*topR) + .587f * (*topG) + .114f * (*topB);
            // Set the lighter color as new
            if (lum1 < lum2) // New value is the other image (with respect to opacity)
            {
                *botB = (byte)(op0 * (*botB) + op1 * (*topB));
                *botG = (byte)(op0 * (*botG) + op1 * (*topG));
                *botR = (byte)(op0 * (*botR) + op1 * (*botR));
            }
        }
    }
}
