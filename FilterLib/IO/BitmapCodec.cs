using System.IO;

namespace FilterLib.IO;

/// <summary>
/// Codec for standard 24 bit per pixel BITMAPINFOHEADER bitmaps.
/// </summary>
public class BitmapCodec : CodecBase
{
    /*
     * Example: a 3×2 pixel, 24-bit bitmap (BITMAPINFOHEADER, RGB24).
     *
     * The image:
     *
     *     +--------+--------+--------+
     *     | Red    | Green  | Blue   |  y=0
     *     +--------+--------+--------+
     *     | White  | Black  | Yellow |  y=1
     *     +--------+--------+--------+
     *       x=0      x=1      x=2
     *
     * Rows are stored bottom-to-top, pixels as BGR, each row padded to a
     * multiple of 4 bytes (3×3 = 9 data bytes + 3 padding = 12 bytes/row).
     *
     * Offset  Size  Hex value      Value          Description
     * ------  ----  -----------    -----------    --------------------------------------
     * BMP header
     * 00      2     42 4D          "BM"           ID field
     * 02      4     4E 00 00 00    78 bytes       Size of the BMP file (54 header + 24 data)
     * 06      2     00 00          Unused         Application specific
     * 08      2     00 00          Unused         Application specific
     * 0A      4     36 00 00 00    54 bytes       Offset where the pixel array can be found
     * DIB header
     * 0E      4     28 00 00 00    40 bytes       Number of bytes in the DIB header
     * 12      4     03 00 00 00    3 pixels       Width of the bitmap (left to right order)
     * 16      4     02 00 00 00    2 pixels       Height (positive = bottom to top order)
     * 1A      2     01 00          1 plane        Number of color planes
     * 1C      2     18 00          24 bits        Number of bits per pixel
     * 1E      4     00 00 00 00    0              BI_RGB, no compression used
     * 22      4     18 00 00 00    24 bytes       Size of the raw pixel data (with padding)
     * 26      4     13 0B 00 00    2835 px/m      Horizontal resolution (72 DPI)
     * 2A      4     13 0B 00 00    2835 px/m      Vertical resolution (72 DPI)
     * 2E      4     00 00 00 00    0 colors       Number of colors in the palette
     * 32      4     00 00 00 00    0 colors       Important colors (0 = all)
     * Start of pixel array (bitmap data)
     * 36      3     FF FF FF       255 255 255    White,  Pixel (x=0, y=1)
     * 39      3     00 00 00       0 0 0          Black,  Pixel (x=1, y=1)
     * 3C      3     00 FF FF       0 255 255      Yellow, Pixel (x=2, y=1)
     * 3F      3     00 00 00       0 0 0          Padding for 4 byte alignment
     * 42      3     00 00 FF       0 0 255        Red,    Pixel (x=0, y=0)
     * 45      3     00 FF 00       0 255 0        Green,  Pixel (x=1, y=0)
     * 48      3     FF 00 00       255 0 0        Blue,   Pixel (x=2, y=0)
     * 4B      3     00 00 00       0 0 0          Padding for 4 byte alignment
     */

    private const uint BMP_HEADER_SIZE = 14;
    private const uint DIB_HEADER_SIZE = 40;
    private const int RESOLUTION = 2835; // 72 DPI converted to pixels/meter

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
        for (int i = 0; i < n; i++) stream.WriteByte(0);
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
        /*uint pixelArraySize =*/
        ReadUint(dibHeader[20..24]);
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
            for (int i = 0; i < padding; ++i)
                if (stream.ReadByte() == -1)
                    throw new EndOfStreamException("Stream ended inside pixel array");
        }
        return img;
    }
}
