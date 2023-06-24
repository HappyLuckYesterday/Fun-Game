using Rhisis.Core.Helpers;

namespace Rhisis.Game;

/// <summary>
/// This class describes the behavior of a Rectangle in a 3D environment.
/// </summary>
/// <remarks>
/// This Rectangle implementation doesn't have a Y property because in a 3D world, Y coordinate represents the heights.
/// </remarks>
public class Rectangle
{
    /// <summary>
    /// Gets the X position of the rectangle.
    /// </summary>
    public int X { get; init; }

    /// <summary>
    /// Gets the Z position of the rectangle.
    /// </summary>
    public int Z { get; init; }

    /// <summary>
    /// Gets the width of the rectangle.
    /// </summary>
    public int Width { get; init; }

    /// <summary>
    /// Gets the length of the rectangle.
    /// </summary>
    public int Length { get; init; }

    /// <summary>
    /// Creates a new <see cref="Rectangle"/> initialized to 0.
    /// </summary>
    protected Rectangle()
    {
    }

    /// <summary>
    /// Creates a new <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="x">X coordinate (X Top Left corner)</param>
    /// <param name="z">Z coordinate (Z top left corner)</param>
    /// <param name="width">Width of the region</param>
    /// <param name="length">Length of the region</param>
    public Rectangle(int x, int z, int width, int length)
    {
        X = x;
        Z = z;
        Width = width;
        Length = length;
    }

    /// <summary>
    /// Generates a random position inside the rectangle.
    /// </summary>
    /// <param name="height">Optional height.</param>
    /// <returns>A new <see cref="Vector3"/></returns>
    public Vector3 GetRandomPosition(float height = 0)
    {
        return new Vector3()
        {
            X = FFRandom.FloatRandom(X, X + Width),
            Y = height,
            Z = FFRandom.FloatRandom(Z, Z + Length)
        };
    }

    /// <summary>
    /// Check if the position passed as parameter is inside the rectangle.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool Contains(Vector3 position) => Contains(position.X, position.Y, position.Z);

    /// <summary>
    /// Check if the possition passed as parameter is inside the rectangle.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="z">Z coordinate.</param>
    /// <returns></returns>
    public bool Contains(float x, float y, float z) => x >= X && x <= X + Width && z >= Z && z <= Z + Length;


}
