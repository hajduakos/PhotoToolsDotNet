﻿using FilterLib.Util;

namespace FilterLib.Blending.Lighten
{
    /// <summary>
    /// Add bottom and top layer.
    /// </summary>
    [Blend]
    public sealed class LinearDodgeBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public LinearDodgeBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override byte BlendComponent(byte compBottom, byte compTop) =>
            (compBottom + compTop).ClampToByte();
    }
}
