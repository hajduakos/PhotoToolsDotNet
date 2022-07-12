namespace FilterLib.Blending
{
    /// <summary>
    /// Normal blend mode.
    /// </summary>
    [Blend]
    public sealed class NormalBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public NormalBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            return compTop;
        }
    }
}
