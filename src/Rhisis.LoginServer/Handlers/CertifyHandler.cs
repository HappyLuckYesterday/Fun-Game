using LiteMessageHandler;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Configuration;
using Rhisis.Core.Cryptography;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets;
using Rhisis.Protocol.Packets.Clients;
using System;
using System.Linq;

namespace Rhisis.LoginServer.Handlers;

[PacketHandler(PacketType.CERTIFY)]
internal sealed class CertifyHandler : LoginPacketHandler, IMessageHandler<CertifyPacket>
{
    private readonly ILogger<CertifyHandler> _logger;
    private readonly IAccountDatabase _accountDatabase;
    private readonly IOptions<LoginServerOptions> _options;

    public CertifyHandler(ILogger<CertifyHandler> logger, IAccountDatabase accountDatabase, IOptions<LoginServerOptions> options)
    {
        _logger = logger;
        _accountDatabase = accountDatabase;
        _options = options;
    }

    public void Execute(CertifyPacket message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        ArgumentException.ThrowIfNullOrEmpty(message.Username);
        ArgumentException.ThrowIfNullOrEmpty(message.BuildVersion);

        if (_options.Value.BuildVersion != message.BuildVersion)
        {
            SendAuthenticationFailed(ErrorType.ILLEGAL_VER, "Invalid client build version.");
            return;
        }

        byte[] passwordEncryptionKey = Aes.BuildEncryptionKeyFromString(_options.Value.PasswordEncryptionKey, 16);
        string password = Aes.DecryptByteArray(message.Password, passwordEncryptionKey);

        AccountEntity account = _accountDatabase.Accounts
            .FirstOrDefault(x => x.Username.Equals(message.Username, StringComparison.OrdinalIgnoreCase) && 
                                 x.Password.Equals(password, StringComparison.OrdinalIgnoreCase));

        if (!VerifyAccount(account))
        {
            return;
        }

        account.LastConnectionTime = DateTime.UtcNow;
        _accountDatabase.Accounts.Update(account);
        _accountDatabase.SaveChanges();

        User.Username = account.Username;
        User.UserId = account.Id;

        // TODO: check if already connected
        // TODO: send cluster server list

        _logger.LogInformation($"User '{account.Username}' logged-in successfuly.");
    }

    private bool VerifyAccount(AccountEntity account)
    {
        if (account is null)
        {
            SendAuthenticationFailed(ErrorType.FLYFF_ACCOUNT, "Account doesn't exists. (Bad username)");
            return false;
        }
        else if (account.IsDeleted)
        {
            SendAuthenticationFailed(ErrorType.ILLEGAL_ACCESS, "Account has been deleted.");
            return false;
        }

        return true;
    }

    private void SendAuthenticationFailed(ErrorType error, string reason = null)
    {
        if (!string.IsNullOrWhiteSpace(reason))
        {
            _logger.LogInformation($"Unable to authenticate user. Reason: {reason}");
        }

        using ErrorPacket packet = new(error);

        User.Send(packet);
    }
}

