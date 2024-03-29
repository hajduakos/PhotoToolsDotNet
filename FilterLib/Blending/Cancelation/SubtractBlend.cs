﻿using FilterLib.Util;

namespace FilterLib.Blending.Cancelation
{
    /// <summary>
    /// Subtract top layer from bottom, negative numbers become black.
    /// </summary>
    [Blend]
    public sealed class SubtractBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public SubtractBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override byte BlendComponent(byte compBottom, byte compTop) =>
            (compBottom - compTop).ClampToByte();
    }
}
