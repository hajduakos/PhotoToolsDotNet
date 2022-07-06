namespace FilterLib
{
    public sealed class Image
    {
        private readonly byte[] data;

        public int Width { get; private set; }

        public int Height { get; private set; }

        public Image(int width, int height)
        {
            Width = width;
            Height = height;
            data = new byte[Width * Height * 3];
        }

        public byte this[int x, int y, int c]
        {
            get { return data[y * Width * 3 + x * 3 + c]; }
            set { data[y * Width * 3 + x * 3 + c] = value; }
        }

        public byte this[int i]
        {
            get { return data[i]; }
            set { data[i] = value; }
        }

        public ref byte GetPinnableReference()
        {
            return ref data[0];
        }
    }
}
