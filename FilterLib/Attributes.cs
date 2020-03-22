using System;

namespace FilterLib
{
    /// <summary>
    /// Attribute for marking classes that are filters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FilterAttribute : Attribute { }


    /// <summary>
    /// Attribute for marking properties that are filter parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterParamAttribute : Attribute { }

    /// <summary>
    /// Attribute for marking the minimum value for a parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterParamMinAttribute : Attribute
    {
        public int Value { get; private set; }

        public FilterParamMinAttribute(int value) => this.Value = value;
    }

    /// <summary>
    /// Attribute for marking the maximum value for a parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterParamMaxAttribute : Attribute
    {
        public int Value { get; private set; }

        public FilterParamMaxAttribute(int value) => this.Value = value;
    }

    /// <summary>
    /// Attribute for marking the minimum value for a parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterParamMinFAttribute : Attribute
    {
        public float Value { get; private set; }

        public FilterParamMinFAttribute(float value) => this.Value = value;
    }

    /// <summary>
    /// Attribute for marking the maximum value for a parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterParamMaxFAttribute : Attribute
    {
        public float Value { get; private set; }

        public FilterParamMaxFAttribute(float value) => this.Value = value;
    }
}
