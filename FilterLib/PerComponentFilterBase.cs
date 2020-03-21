namespace FilterLib
{
    public abstract class PerComponentFilterBase : PerPixelFilterBase
    {
        private byte[] map = null;
        protected abstract byte MapComponent(byte comp);

        protected override void ApplyStart()
        {
            base.ApplyStart();
            map = new byte[256];
            byte i = 0;
            do
            {
                map[i] = MapComponent(i);
            } while (i++ != 255);
        }

        protected override unsafe void ProcessPixel(byte* r, byte* g, byte* b)
        {
            *r = map[*r];
            *g = map[*g];
            *b = map[*b];
        }

        protected override void ApplyEnd()
        {
            map = null;
            base.ApplyEnd();
        }

    }
}
