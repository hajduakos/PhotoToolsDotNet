using System;

namespace FilterLib.Util
{
    /// <summary>
    /// Represents a size that can be absolute (px) or relative (%).
    /// </summary>
    public abstract class Size
    {
        private Size() { }

        /// <summary>
        /// Convert the size to absoute with repsect to a reference.
        /// </summary>
        /// <param name="reference">Reference</param>
        /// <returns></returns>
        public abstract int ToAbsolute(int reference);

        /// <summary>
        /// Create new absolute size.
        /// </summary>
        /// <param name="value">Size in pixels</param>
        /// <returns></returns>
        public static Size Absolute(int value) => new AbsoluteSize(value);

        /// <summary>
        /// Create new relative size.
        /// </summary>
        /// <param name="percentage">Size in percentage (1 is 100%)</param>
        /// <returns></returns>
        public static Size Relative(float percentage) => new RelativeSize(percentage);

        /// <summary>
        /// Parse size of format '123px' or '123%'.
        /// </summary>
        /// <param name="str">String to be parsed</param>
        /// <returns>A new absolute or relative size</returns>
        public static Size FromString(string str)
        {
            str = str.Trim().ToLower();
            if (str.EndsWith('%'))
                return Relative(float.Parse(str[0..^1].Trim(), System.Globalization.CultureInfo.InvariantCulture.NumberFormat) / 100);
            else if (str.EndsWith("px"))
                return Absolute(int.Parse(str[0..^2].Trim()));
            throw new FormatException("Size must end with '%' or 'px' as unit.");
        }

        private class AbsoluteSize : Size
        {
            private readonly int val;
            public AbsoluteSize(int val) => this.val = val;

            public override int ToAbsolute(int reference) => val;
        }

        private class RelativeSize : Size
        {
            private readonly float pct;
            public RelativeSize(float percentage) => this.pct = percentage;

            public override int ToAbsolute(int reference) => (int)(reference * pct);
        }
    }
}
