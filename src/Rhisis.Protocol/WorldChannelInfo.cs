namespace Rhisis.Protocol;

public sealed class WorldChannelInfo
{
    public int Id { get; set; }

    public string Ip { get; set; }

    public int Port { get; set; }

    public string Name { get; set; }

    public int MaximumUsers { get; set; }

    public int ConnectedUsers { get; set; }

    public bool IsEnabled { get; set; }
}
