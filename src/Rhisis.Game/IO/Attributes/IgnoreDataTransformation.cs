using System;

namespace Rhisis.Game.IO.Attributes;

/// <summary>
/// When applied to the property of a type, specifies that the property should not be transformed.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class IgnoreDataTransformation : Attribute
{
}