using System;
using System.Text.RegularExpressions;

namespace FilterLib.Util
{
    /// <summary>
    /// 3x3 convolution matrix structure.
    /// </summary>
    public struct Conv3x3
    {
        private readonly int[,] matrix;

        /// <summary>
        /// Index the matrix.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public int this[int i, int j]
        {
            get { return matrix[i, j]; }
        }

        /// <summary> Divisor </summary>
        public readonly int Divisor;

        /// <summary> Bias </summary>
        public readonly int Bias;

        /// <summary>
        /// Constructor with the matrix, divisor and bias parameters </summary>
        /// <param name="tl">Top left element</param>
        /// <param name="tm">Top middle element</param>
        /// <param name="tr">Top right element</param>
        /// <param name="ml">Middle left element</param>
        /// <param name="mm">Middle middle element</param>
        /// <param name="mr">Middle right element</param>
        /// <param name="bl">Bottom left element</param>
        /// <param name="bm">Bottom middle element</param>
        /// <param name="br">Bottom right element</param>
        /// <param name="divisor">Divisor value</param>
        /// <param name="bias">Bias value</param>
        public Conv3x3(int tl, int tm, int tr, int ml, int mm, int mr, int bl, int bm, int br, int divisor = 1, int bias = 0)
        {
            matrix = new int[3, 3]; // 2D New array
            matrix[0, 0] = tl; matrix[1, 0] = tm; matrix[2, 0] = tr; // Assign elements
            matrix[0, 1] = ml; matrix[1, 1] = mm; matrix[2, 1] = mr;
            matrix[0, 2] = bl; matrix[1, 2] = bm; matrix[2, 2] = br;
            Divisor = (divisor == 0) ? 1 : divisor; // Divisor (can't be 0)
            this.Bias = bias; // Bias
        }

        /// <summary>
        /// Parse a matrix from a string of format [a b c ; d e f ; g h i] / d + b
        /// </summary>
        /// <param name="str">String to be parsed</param>
        public Conv3x3(string str)
        {
            str = str.Replace(';', ' ');
            str = Regex.Replace(str, @"\s+", " ").Trim();
            int openbracket = str.IndexOf('[');
            if (openbracket < 0) throw new FormatException("Expected '['.");
            int closebracket = str.IndexOf(']');
            if (closebracket < openbracket) throw new FormatException("Expected ']' after '['");
            string[] mx = str.Substring(openbracket + 1, closebracket - openbracket - 1).Split(' ');

            matrix = new int[3, 3]; // 2D New array
            matrix[0, 0] = int.Parse(mx[0]); matrix[1, 0] = int.Parse(mx[1]); matrix[2, 0] = int.Parse(mx[2]); // Assign elements
            matrix[0, 1] = int.Parse(mx[3]); matrix[1, 1] = int.Parse(mx[4]); matrix[2, 1] = int.Parse(mx[5]);
            matrix[0, 2] = int.Parse(mx[6]); matrix[1, 2] = int.Parse(mx[7]); matrix[2, 2] = int.Parse(mx[8]);

            int slash = str.IndexOf('/');
            if (slash < 0) throw new FormatException("Expected '/'");
            int plus = str.IndexOf('+');
            if (plus < slash) throw new FormatException("Expected '+' after '/'");
            Divisor = int.Parse(str.Substring(slash + 1, plus - slash - 1).Trim());
            if (Divisor == 0) Divisor = 1;
            Bias = int.Parse(str.Substring(plus + 1).Trim());
        }

        /// <summary>
        /// Create a copy of the matrix array.
        /// </summary>
        /// <returns>A new copy of the matrix array</returns>
        public int[,] CopyMatrix()
        {
            int[,] copy = new int[3, 3];
            Array.Copy(matrix, copy, 3 * 3);
            return copy;
        }

        /// <summary>
        /// Convert to string </summary>
        /// <returns>String representation of the convolution matrix</returns>
        public override string ToString()
        {
            return "[" + matrix[0, 0] + " " + matrix[1, 0] + " " + matrix[2, 0] + " ; " +
                   matrix[0, 1] + " " + matrix[1, 1] + " " + matrix[2, 1] + " ; " +
                   matrix[0, 2] + " " + matrix[1, 2] + " " + matrix[2, 2] + "] / " + Divisor + " + " + Bias;
        }
    }
}