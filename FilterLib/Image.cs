using Debug = System.Diagnostics.Debug;

namespace FilterLib
{
    /// <summary>
    /// Represents an image with a particular width and height in the RGB color
    /// space with 1 byte for each component (3 bytes per pixel).
    /// </summary>
    public sealed class Image : System.ICloneable
    {
        private readonly byte[] data;

        /// <summary>
        /// Width of the image in pixels.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height of the image in pixels.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Create a new, blank image.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public Image(int width, int height)
        {
            Width = width;
            Height = height;
            data = new byte[Width * Height * 3];
        }

        /// <summary>
        /// Get a given component of a given pixel. Checks on bounds are only performed in debug mode.
        /// </summary>
        /// <param name="x">X coordinate of the pixel, must be between 0 and Width-1</param>
        /// <param name="y">Y coordinate of the pixel, must be between 0 and Height-1</param>
        /// <param name="c">Component index, must be 0 (Red), 1 (Green) or 2 (Blue)</param>
        /// <returns></returns>
        public byte this[int x, int y, int c]
        {
            get
            {
                Debug.Assert(x >= 0, "x must be >= 0.");
                Debug.Assert(x < Width, "x must be < Width.");
                Debug.Assert(y >= 0, "y must be >= 0.");
                Debug.Assert(y < Height, "y must be < Height.");
                Debug.Assert(c >= 0, "c must be >= 0.");
                Debug.Assert(c < 3, "c must be < 3.");
                return data[y * Width * 3 + x * 3 + c];
            }
            set
            {
                Debug.Assert(x >= 0, "x must be >= 0.");
                Debug.Assert(x < Width, "x must be < Width.");
                Debug.Assert(y >= 0, "y must be >= 0.");
                Debug.Assert(y < Height, "y must be < Height.");
                Debug.Assert(c >= 0, "c must be >= 0.");
                Debug.Assert(c < 3, "c must be < 3.");
                data[y * Width * 3 + x * 3 + c] = value;
            }
        }

        /// <summary>
        /// Get a pinnable reference (to be used in fixed statements) to the internal data
        /// for faster access. The image is represented as a 1D array of its rows, where each
        /// row has 3 bytes (R, G, B) for each pixel.
        /// </summary>
        /// <returns>Reference</returns>
        public ref byte GetPinnableReference() => ref data[0];

        /// <inheritdoc/>
        public object Clone()
        {
            Image img = new(Width, Height);
            System.Array.Copy(data, img.data, data.Length);
            return img;
        }
    }
}
