using System;

namespace FilterLib.Blending;

/// <summary>
/// Attribute for marking classes that are blend modes.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class BlendAttribute(string description = "") : Attribute
{
    public string Description { get; } = description;
}
