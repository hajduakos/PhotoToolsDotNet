namespace FilterLib.IO
{
    /// <summary>
    /// Interface for image codecs, providing functions to read/write images of certain formats.
    /// </summary>
    public interface ICodec
    {
        /// <summary>
        /// Write the image to a given stream.
        /// </summary>
        /// <param name="img">Image</param>
        /// <param name="stream">Stream</param>
        public void Write(Image img, System.IO.Stream stream);

        /// <summary>
        /// Write the image to a file.
        /// </summary>
        /// <param name="img">Image</param>
        /// <param name="filename">Path to file</param>
        public void WriteFile(Image img, string filename);

        /// <summary>
        /// Write the image into a memory array.
        /// </summary>
        /// <param name="img">Image</param>
        /// <returns>Memory array</returns>
        public byte[] WriteArray(Image img);
    }
}
