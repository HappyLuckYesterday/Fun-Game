using System;

namespace Rhisis.Game.Common
{
    [Flags]
    public enum ItemFlags : uint
    {
        Expired = 0x01,
        Binds = 0x02,
        IsUsing = 0x04,
    }
}
