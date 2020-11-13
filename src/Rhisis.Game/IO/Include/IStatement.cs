using System;

namespace Rhisis.Game.IO.Include
{
    public interface IStatement : IDisposable
    {
        string Name { get; }

        StatementType Type { get; }
    }
}
