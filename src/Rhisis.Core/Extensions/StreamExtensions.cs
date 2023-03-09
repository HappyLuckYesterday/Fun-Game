using System;
using System.IO;

namespace Rhisis.Core.Extensions;

/// <summary>
/// Provides extensions for the <see cref="Stream"/> object.
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Checks if the stream is at its end.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <returns>True if the stream is at its end; false otherwise.</returns>
    public static bool IsEndOfStream(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        return stream.Position >= stream.Length;
    }
}