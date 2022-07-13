namespace FilterLib.Blending
{
    /// <summary>
    /// Pick the darker value for each component.
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
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop) =>
            System.Math.Min(compBottom, compTop);
    }
}
