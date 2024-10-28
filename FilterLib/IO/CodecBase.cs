using System.IO;

namespace FilterLib.IO
{
    /// <summary>
    /// Base class for codecs.
    /// </summary>
    public abstract class CodecBase : ICodec
    {
        /// <inheritdoc/>
        public abstract void Write(Image img, Stream stream);

        /// <inheritdoc/>
        public void WriteFile(Image img, string filename)
        {
            using Stream stream = File.OpenWrite(filename);
            Write(img, stream);
        }

        /// <inheritdoc/>
        public byte[] WriteArray(Image img)
        {
            using MemoryStream stream = new();
            Write(img, stream);
            return stream.ToArray();
        }
    }
}
