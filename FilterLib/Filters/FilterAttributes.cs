using System;

namespace FilterLib.Filters;

/// <summary>
/// Attribute for marking classes that are filters.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class FilterAttribute(string description = "") : Attribute
{
    public string Description { get; } = description;
}


/// <summary>
/// Attribute for marking properties that are filter parameters.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FilterParamAttribute : Attribute { }

/// <summary>
/// Attribute for marking the minimum value for a parameter.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FilterParamMinAttribute(int value) : Attribute
{
    public int Value { get; } = value;
}

/// <summary>
/// Attribute for marking the maximum value for a parameter.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FilterParamMaxAttribute(int value) : Attribute
{
    public int Value { get; } = value;
}

/// <summary>
/// Attribute for marking the minimum value for a parameter.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FilterParamMinFAttribute(float value) : Attribute
{
    public float Value { get; } = value;
}

/// <summary>
/// Attribute for marking the maximum value for a parameter.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FilterParamMaxFAttribute(float value) : Attribute
{
    public float Value { get; } = value;
}
