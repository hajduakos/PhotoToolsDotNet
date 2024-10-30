namespace FilterLib.IO
{
    /// <summary>
    /// Base class for codecs.
    /// </summary>
    public abstract class CodecBase : ICodec
    {
        /// <inheritdoc/>
        public abstract void Write(Image img, System.IO.Stream stream);

        /// <inheritdoc/>
        public void Write(Image img, string filename)
        {
            using System.IO.Stream stream = System.IO.File.OpenWrite(filename);
            Write(img, stream);
        }

        /// <inheritdoc/>
        public byte[] Write(Image img)
        {
            using System.IO.MemoryStream stream = new();
            Write(img, stream);
            return stream.ToArray();
        }
    }
}
