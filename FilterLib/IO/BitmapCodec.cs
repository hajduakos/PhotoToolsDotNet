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

        private static uint ReadUint(byte[] bytes)
        {
            if (!System.BitConverter.IsLittleEndian) System.Array.Reverse(bytes);
            return System.BitConverter.ToUInt32(bytes, 0);
        }

        private static int ReadInt(byte[] bytes)
        {
            if (!System.BitConverter.IsLittleEndian) System.Array.Reverse(bytes);
            return System.BitConverter.ToInt32(bytes, 0);
        }

        private static ushort ReadUshort(byte[] bytes)
        {
            if (!System.BitConverter.IsLittleEndian) System.Array.Reverse(bytes);
            return System.BitConverter.ToUInt16(bytes, 0);
        }

        /// <inheritdoc/>
        public override Image Read(Stream stream)
        {
            byte[] bmpHeader = new byte[BMP_HEADER_SIZE];
            int cnt = stream.Read(bmpHeader, 0, bmpHeader.Length);
            if (cnt < bmpHeader.Length)
                throw new EndOfStreamException($"Expected {bmpHeader.Length} bytes BMP header but got only {cnt}");
            if (bmpHeader[0] != 0x42 || bmpHeader[1] != 0x4D)
                throw new CodecException("Invalid start of BMP header");
            uint pixelArrayOffset = ReadUint(bmpHeader[10..14]);
            byte[] dibHeader = new byte[DIB_HEADER_SIZE];
            cnt = stream.Read(dibHeader, 0, dibHeader.Length);
            if (cnt < dibHeader.Length)
                throw new EndOfStreamException($"Expected {dibHeader.Length} bytes DIB header but got only {cnt}");
            uint dibHeaderSize = ReadUint(dibHeader[0..4]);
            if (dibHeaderSize != DIB_HEADER_SIZE)
                throw new CodecException($"Expected {DIB_HEADER_SIZE} bytes DIB header but got {dibHeaderSize}");
            int width = ReadInt(dibHeader[4..8]);
            int height = ReadInt(dibHeader[8..12]);
            ushort colorPlanes = ReadUshort(dibHeader[12..14]);
            if (colorPlanes != 1)
                throw new CodecException($"Unsupported number of color planes: {colorPlanes}");
            ushort bpp = ReadUshort(dibHeader[14..16]);
            if (bpp != 24)
                throw new CodecException($"Unsupported bits per pixel: {bpp}");
            if (ReadUint(dibHeader[16..20]) != 0)
                throw new CodecException("Unsupported compression");
            /*uint pixelArraySize =*/ ReadUint(dibHeader[20..24]);
            // Ignore horizontal resolution (bytes 24..28)
            // Ignore vertical resolution (bytes 28..32)
            if (ReadUint(dibHeader[32..36]) != 0)
                throw new CodecException("Color palettes are not supported");
            if (ReadUint(dibHeader[36..40]) != 0)
                throw new CodecException("Important colors are not supported");
            Image img = new(width, height);
            if (pixelArrayOffset < BMP_HEADER_SIZE + DIB_HEADER_SIZE)
                throw new CodecException("Pixel array offset points inside header");
            uint totalRead = BMP_HEADER_SIZE + DIB_HEADER_SIZE;
            while (totalRead < pixelArrayOffset)
            {
                if (stream.ReadByte() == -1)
                    throw new EndOfStreamException("Stream ended before pixel array");
                ++totalRead;
            }
            int padding = (int)GetRowSize(img) - img.Width * 3;
            byte[] bgr = new byte[3];
            for (int y = img.Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < img.Width; ++x)
                {
                    cnt = stream.Read(bgr, 0, 3);
                    if (cnt < 3)
                        throw new EndOfStreamException("Stream ended inside pixel array");
                    img[x, y, 0] = bgr[2];
                    img[x, y, 1] = bgr[1];
                    img[x, y, 2] = bgr[0];
                }
                for (int i = 0; i <padding; ++i)
                    if (stream.ReadByte() == -1)
                        throw new EndOfStreamException("Stream ended inside pixel array");
            }
             return img;
        }
    }
}
