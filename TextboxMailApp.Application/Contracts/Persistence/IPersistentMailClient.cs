using MailKit.Net.Imap;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Contracts.Persistence
{
    public interface IPersistentMailClient
    {
        Task<ImapClient> GetConnectedClientAsync(User user);

    }
}
