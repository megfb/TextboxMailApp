using TextboxMailApp.Application.Features.EmailMessages;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Contracts.Persistence
{
    public interface IEmailReader
    {
        Task<List<EmailMessagesDto>> GetEmailsByPageAsync(int page, int pageSize,User user);
        Task<List<EmailMessagesDto>> GetEmailsAfterUidAsync(uint lastMaxUid,User user);

    }
}
