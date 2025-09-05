using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Security;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence
{
    public class PersistentMailClient : IPersistentMailClient
    {
        private readonly ImapClient _client;
        private bool _isConnected = false;
        private User? _currentUser;
        public PersistentMailClient()
        {
            _client = new ImapClient();
        }
        public async Task<ImapClient> GetConnectedClientAsync(User user)
        {
            if (_isConnected && _client.IsConnected && _client.IsAuthenticated)
            {
                return _client;
            }
            await _client.ConnectAsync(user.ServerName, user.Port, SecureSocketOptions.SslOnConnect);
            await _client.AuthenticateAsync(user.EmailAddress, user.EmailPassword);

            _isConnected = true;
            _currentUser = user;

            return _client;
        }
    }
}
