using Microsoft.EntityFrameworkCore;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence.EmailMessages
{
    public class EmailMessageRepository(AppDbContext appDbContext) : GenericRepository<EmailMessage>(appDbContext), IEmailMessageRepository
    {
        private readonly DbSet<EmailMessage> _dbSet = appDbContext.Set<EmailMessage>();

        public override async Task<IEnumerable<EmailMessage>> GetAllByPageAsync(int pageNumber, int pageSize, string userId)
        {
            return await _dbSet.Where(x => x.UserId == userId).OrderByDescending(e => e.Uid).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<EmailMessage?> GetLatestAsync(string id)
        {
            return await _dbSet.Where(x => x.UserId == id).OrderByDescending(e => e.Uid).FirstOrDefaultAsync();
        }

        public async Task<uint?> GetMinUidAsync(string userId)
        {
            return await _dbSet.Where(x => x.UserId == userId).MinAsync(x => (uint?)x.Uid);
        }
    }
}
