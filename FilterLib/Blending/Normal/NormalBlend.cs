namespace FilterLib.Blending.Normal
{
    /// <summary>
    /// Use the top layer without any arithmetic.
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
        protected override byte BlendComponent(byte compBottom, byte compTop) => compTop;
    }
}
