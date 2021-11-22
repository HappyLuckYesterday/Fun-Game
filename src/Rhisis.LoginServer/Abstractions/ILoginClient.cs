using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.LoginServer.Abstractions
{
    public interface ILoginClient
    {
        /// <summary>
        /// Gets the ID assigned to this session.
        /// </summary>
        uint SessionId { get; }

        /// <summary>
        /// Gets the client's logged user id.
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// Gets the client's logged username.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Check if the client is connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Disconnects the current client.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Disconnects the current client with a reason.
        /// </summary>
        /// <param name="reason"></param>
        void Disconnect(string reason);

        /// <summary>
        /// Sets the client's username and id.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userId"></param>
        void SetClientUsername(string username, int userId);

        /// <summary>
        /// Sends a packet to the current client.
        /// </summary>
        /// <param name="packet"></param>
        void Send(ILitePacketStream packet);
    }
}