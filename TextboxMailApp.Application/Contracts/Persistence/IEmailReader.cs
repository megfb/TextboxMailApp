using TextboxMailApp.Application.Features.EmailMessages;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Contracts.Persistence
{
    public interface IEmailReader
    {
        Task<List<EmailMessagesDto>> GetEmailsByPageAsync(User user, uint? minExistingUid = null);
        Task<List<EmailMessagesDto>> GetEmailsAfterUidAsync(uint lastMaxUid, User user);

    }
}
