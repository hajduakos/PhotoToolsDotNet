namespace FilterLib.Blending
{
    /// <summary>
    /// Darken blend mode.
    /// </summary>
    [Blend]
    public sealed class DarkenBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public DarkenBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            return System.Math.Min(compBottom, compTop);
        }
    }
}
