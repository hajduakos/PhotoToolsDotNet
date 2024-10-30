using System.IO;

namespace FilterLib.IO
{
    /// <summary>
    /// Base class for codecs.
    /// </summary>
    public abstract class CodecBase : ICodec
    {
        /// <inheritdoc/>
        public abstract Image Read(Stream stream);

        /// <inheritdoc/>
        public Image Read(string filename)
        {
            using Stream stream = File.OpenRead(filename);
            return Read(stream);
        }

        /// <inheritdoc/>
        public Image Read(byte[] data)
        {
            using MemoryStream stream = new(data);
            return Read(stream);
        }

        /// <inheritdoc/>
        public abstract void Write(Image img, Stream stream);

        /// <inheritdoc/>
        public void Write(Image img, string filename)
        {
            using Stream stream = File.OpenWrite(filename);
            Write(img, stream);
        }

        /// <inheritdoc/>
        public byte[] Write(Image img)
        {
            using MemoryStream stream = new();
            Write(img, stream);
            return stream.ToArray();
        }
    }
}
