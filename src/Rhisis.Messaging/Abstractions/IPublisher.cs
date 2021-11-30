using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhisis.Messaging.Abstractions
{
    public interface IPublisher
    {
    }

    public interface ISubscriber
    {
    }

    public interface IMessage<TValue>
    {
        object Header { get; }

        TValue Content { get; }
    }
}
