﻿using FilterLib.Util;

namespace FilterLib.Blending.Darken
{
    /// <summary>
    /// Multiply top and bottom layers.
    /// </summary>
    [Blend]
    public sealed class MultiplyBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public MultiplyBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override byte BlendComponent(byte compBottom, byte compTop) =>
            (compBottom * compTop / 255f).ClampToByte();
    }
}
