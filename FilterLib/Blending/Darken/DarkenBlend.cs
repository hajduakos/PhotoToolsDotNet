﻿namespace FilterLib.Blending.Darken
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
        protected override byte BlendComponent(byte compBottom, byte compTop) =>
            System.Math.Min(compBottom, compTop);
    }
}
