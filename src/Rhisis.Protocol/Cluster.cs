using System.Collections.Generic;

namespace Rhisis.Protocol;

public sealed class Cluster
{
    public int Id { get; set; }

    public string Ip { get; set; }

    public int Port { get; set; }

    public string Name { get; set; }

    public bool IsEnabled { get; set; }

    public IList<WorldChannel> Channels { get; set; } = new List<WorldChannel>();
}
