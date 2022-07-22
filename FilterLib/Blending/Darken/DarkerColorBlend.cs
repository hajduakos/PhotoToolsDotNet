namespace FilterLib.Blending.Darken
{
    /// <summary>
    /// For each pixel, pick the darker color (based on luminance).
    /// </summary>
    [Blend]
    public sealed class DarkerColorBlend : PerPixelBlendBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public DarkerColorBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override (byte, byte, byte) BlendPixel(byte botR, byte botG, byte botB, byte topR, byte topG, byte topB)
        {
            if (Util.RGB.GetLuminance(botR, botG, botB) > Util.RGB.GetLuminance(topR, topG, topB))
                return (topR, topG, topB);
            else
                return (botR, botG, botB);
        }
    }
}
