using System;

namespace Rhisis.Core;
public class Singleton<TObject> where TObject : class, new()
{
    private static readonly Lazy<TObject> _singletonInstance = new(() => new TObject());

    /// <summary>
    /// Gets the current instance.
    /// </summary>
    public static TObject Current => _singletonInstance.Value;

    protected Singleton()
    {
    }
}
