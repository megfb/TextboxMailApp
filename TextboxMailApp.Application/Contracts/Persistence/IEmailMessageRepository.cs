using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Contracts.Persistence
{
    public interface IEmailMessageRepository : IGenericRepository<EmailMessage>
    {
        Task<EmailMessage?> GetLatestAsync(string id);
        Task<uint?> GetMinUidAsync(string userId);

    }
}
