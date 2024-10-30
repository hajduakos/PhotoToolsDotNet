using System.IO;

namespace FilterLib.IO
{
    /// <summary>
    /// Codec for standard 24 bit per pixel BITMAPINFOHEADER bitmaps.
    /// </summary>
    public class BitmapCodec : CodecBase
    {
        private const uint BMP_HEADER_SIZE = 14;
        private const uint DIB_HEADER_SIZE = 40;
        private const int RESOLUTION = 2835; // 72 DPI converted to pixels/metre

        private static uint GetRowSize(Image img)
        {
            uint w = (uint)img.Width * 3;
            if (w % 4 == 0) return w;
            return w + 4 - (w % 4);
        }

        private static void WriteLittleEndianBytes(Stream stream, byte[] bytes)
        {
            if (!System.BitConverter.IsLittleEndian) System.Array.Reverse(bytes);
            stream.Write(bytes, 0, bytes.Length);
        }

        private static void WriteUint(Stream stream, uint i) =>
            WriteLittleEndianBytes(stream, System.BitConverter.GetBytes(i));

        private static void WriteInt(Stream stream, int i) =>
            WriteLittleEndianBytes(stream, System.BitConverter.GetBytes(i));

        private static void WriteUshort(Stream stream, ushort i) =>
            WriteLittleEndianBytes(stream, System.BitConverter.GetBytes(i));

        private static void WriteZeros(Stream stream, int n)
        {
            for (int i=0; i < n; i++) stream.WriteByte(0);
        }

        private static void WriteBMPHeader(Image img, Stream stream)
        {
            stream.WriteByte(0x42); // B (1 byte)
            stream.WriteByte(0x4D); // M (1 byte)
            uint size = BMP_HEADER_SIZE + DIB_HEADER_SIZE + (uint)img.Height * GetRowSize(img);
            WriteUint(stream, size); // Total size of file (4 bytes)
            WriteZeros(stream, 2); // Unused app specific (2 bytes)
            WriteZeros(stream, 2); // Unused app specific (2 bytes)
            WriteUint(stream, BMP_HEADER_SIZE + DIB_HEADER_SIZE); // Offset to pixel array (4 bytes)
        }

        private static void WriteDIBHeader(Image img, Stream stream)
        {
            WriteUint(stream, DIB_HEADER_SIZE); // DIB header size (4 bytes)
            WriteInt(stream, img.Width); // Width (4 bytes)
            WriteInt(stream, img.Height); // Height, positive for bottom to top order (4 bytes)
            WriteUshort(stream, 1); // Color planes: 1 (2 bytes)
            WriteUshort(stream, 24); // Bits per pixel (2 bytes)
            WriteZeros(stream, 4); // BI_RGB, no compression (4 bytes)
            WriteUint(stream, (uint)img.Height * GetRowSize(img)); // Size of pixel array (4 bytes)
            WriteInt(stream, RESOLUTION); // Horizontal resolution (4 bytes)
            WriteInt(stream, RESOLUTION); // Vertical resolution (4 bytes)
            WriteZeros(stream, 4); // 0 colors in palette (4 bytes)
            WriteZeros(stream, 4); // 0 important colors (4 bytes)
        }

        private static void WritePixelArray(Image img, Stream stream)
        {
            int padding = (int)GetRowSize(img) - img.Width * 3;
            for (int y = img.Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < img.Width; ++x)
                {
                    // BGR order
                    for (int c = 2; c >= 0; --c) stream.WriteByte(img[x, y, c]);
                }
                WriteZeros(stream, padding);
            }
        }

        /// <inheritdoc/>
        public override void Write(Image img, Stream stream)
        {
            WriteBMPHeader(img, stream);
            WriteDIBHeader(img, stream);
            WritePixelArray(img, stream);
        }
    }
}
