﻿using FilterLib.Util;

namespace FilterLib.Blending
{
    /// <summary>
    /// Add layers and subtract double of their multiplication.
    /// </summary>
    [Blend]
    public sealed class ExcludeBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public ExcludeBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop) =>
            (compTop + compBottom - 2 * compBottom * compTop / 255f).ClampToByte();
    }
}
