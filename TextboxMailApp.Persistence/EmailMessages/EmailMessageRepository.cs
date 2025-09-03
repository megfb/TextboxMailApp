using Microsoft.EntityFrameworkCore;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.EmailMessages;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence.EmailMessages
{
    public class EmailMessageRepository(AppDbContext appDbContext) : GenericRepository<EmailMessage>(appDbContext), IEmailMessageRepository
    {
        private readonly DbSet<EmailMessage> _dbSet = appDbContext.Set<EmailMessage>();
        public override async Task<IEnumerable<EmailMessage>> GetAllByPageAsync(int pageNumber, int pageSize)
        {
            return await _dbSet.OrderByDescending(e => e.Uid).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<EmailMessage?> GetLatestAsync()
        {
            return await _dbSet.OrderByDescending(e => e.Uid).FirstOrDefaultAsync();
        }
    }
}
