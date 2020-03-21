using System;

namespace FilterLib.Util
{
    /// <summary>
    /// Utility class for extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Clamp a value into a range defined by its minimum and maximum.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="min">Minimum</param>
        /// <param name="max">Maximum</param>
        /// <returns>Clamped value</returns>
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable
        {
            if (value.CompareTo(max) > 0) return max;
            if (value.CompareTo(min) < 0) return min;
            return value;
        }
    }
}
