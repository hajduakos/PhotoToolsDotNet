﻿using FilterLib.Util;

namespace FilterLib.Blending.Contrast
{
    /// <summary>
    /// Set each component to fully bright or dark based on the bottom and top components.
    /// </summary>
    [Blend]
    public sealed class HardMixBlend : PerComponentBlendBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opacity">Opacity [0:100]</param>
        public HardMixBlend(int opacity = 100) : base(opacity) { }

        /// <inheritdoc/>
        protected override unsafe byte BlendComponent(byte compBottom, byte compTop)
        {
            if (compTop < 255 - compBottom) return 0;
            return 255;
        }
    }
}
