using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FilterLib.Util
{
    /// <summary>
    /// Convoluion matrix with a 2 dimensional matrix of weights, a divisor
    /// and a bias. The center of the matrix is assumed to be the element
    /// at index floor(Width/2), floor(Height/2).
    /// </summary>
    public readonly struct ConvolutionMatrix
    {
        private readonly int[,] weights;

        /// <summary>
        /// Index into the matrix representing the weights.
        /// </summary>
        /// <param name="x">Column index</param>
        /// <param name="y">Row index</param>
        /// <returns></returns>
        public readonly int this[int x, int y] => weights[x, y];

        /// <summary> Width of the matrix </summary>
        public readonly int Width;

        /// <summary> Height of the matrix </summary>
        public readonly int Height;

        /// <summary> Divisor applied to the weighted sum </summary>
        public readonly int Divisor;

        /// <summary> Bias </summary>
        public readonly int Bias;

        /// <summary>
        /// Default constructor, representing the identity function
        /// </summary>
        public ConvolutionMatrix() : this(new int[,] { { 1 } }, 1, 0) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="matrix">Weights as an array of columns</param>
        /// <param name="divisor">Divisor</param>
        /// <param name="bias">Bias</param>
        public ConvolutionMatrix(int[,] matrix, int divisor = 1, int bias = 0)
        {
            Width = matrix.GetLength(0);
            Height = matrix.GetLength(1);
            this.weights = new int[Width, Height];
            for (int x = 0; x < Width; ++x)
                for (int y = 0; y < Height; ++y)
                    this.weights[x, y] = matrix[x, y];
            if (divisor == 0)
                throw new ArithmeticException("Divisor cannot be 0.");
            Divisor = divisor;
            Bias = bias;
        }

        /// <summary>
        /// Parse a matrix from a string of format "[[a, b, c], [d, e, f]] / d + b",
        /// that is, the matrix of weights as a list of columns, divisor and bias.
        /// </summary>
        /// <param name="str">String to be parsed</param>
        public ConvolutionMatrix(string str)
        {
            str = Regex.Replace(str, @"\s+", " ").Trim();
            Match match = Regex.Match(str, @"\[(.*)\]\s*\/\s*(-?\d+)\s*\+\s*(-?\d+)");
            if (!match.Success) throw new ArgumentException("String does not match expected format");
            List<List<int>> mx = [];
            Match subMatch = Regex.Match(match.Groups[1].Value, @"\[([^\]]+)\]");
            while (subMatch.Success)
            {
                List<int> col = [];
                foreach (string s in subMatch.Groups[1].Value.Split(", "))
                    col.Add(int.Parse(s));
                mx.Add(col);
                subMatch = subMatch.NextMatch();
            }
            Divisor = int.Parse(match.Groups[2].Value);
            if (Divisor == 0)
                throw new ArithmeticException("Divisor cannot be 0.");
            Bias = int.Parse(match.Groups[3].Value);

            Width = mx.Count;
            if (Width == 0) throw new ArgumentException("Matrix cannot be empty");
            Height = mx[0].Count;
            if (Height == 0) throw new ArgumentException("Columns cannot be empty");

            weights = new int[Width,Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    weights[x, y] = mx[x][y];
        }

        /// <summary>
        /// Convert to string </summary>
        /// <returns>
        /// String representation of the convolution matrix, as a list of columns
        /// </returns>
        public override readonly string ToString()
        {
            StringBuilder sb = new();
            sb.Append('[');
            for (int x = 0; x < Width; x++)
            {
                sb.Append('[');
                for (int y = 0; y < Height; y++)
                {
                    sb.Append(this[x, y]);
                    if (y != Height - 1) sb.Append(", ");
                }
                sb.Append(']');
                if (x != Width - 1) sb.Append(", ");
            }
            sb.Append("] / ");
            sb.Append(Divisor);
            sb.Append(" + ");
            sb.Append(Bias);
            return sb.ToString();
        }
    }
}