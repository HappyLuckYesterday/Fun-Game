namespace Rhisis.WorldServer.Abstractions;

public interface IWorldChannel
{
    string Name { get; }

    string Ip { get; }

    int Port { get; }

    string Cluster { get; }
}
