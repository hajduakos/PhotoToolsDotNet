namespace FilterLib.Filters
{
    /// <summary>
    /// Base class for filters that process each R,G,B component independently.
    /// Uses caching to pre-compute all possible values for faster application.
    /// </summary>
    public abstract class PerComponentFilterBase : PerPixelFilterBase
    {
        private byte[] map = null;

        /// <summary>
        /// Map a single (R/G/B) component.
        /// </summary>
        /// <param name="comp">Input value</param>
        /// <returns>Output value by applying the filter</returns>
        protected abstract byte MapComponent(byte comp);

        /// <inheritdoc/>
        protected override sealed void ApplyStart()
        {
            base.ApplyStart();
            // Pre-calculate and cache all the possibilities
            map = new byte[256];
            byte i = 0;
            do
            {
                map[i] = MapComponent(i);
            } while (i++ != 255);
        }

        /// <inheritdoc/>
        protected override sealed unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            System.Diagnostics.Debug.Assert(map != null);
            // Just use cache
            *r = map[*r];
            *g = map[*g];
            *b = map[*b];
        }

        /// <inheritdoc/>
        protected override sealed void ApplyEnd()
        {
            map = null;
            base.ApplyEnd();
        }
    }
}
