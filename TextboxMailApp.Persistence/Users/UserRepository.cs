using Microsoft.EntityFrameworkCore;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence.Users
{
    public class UserRepository(AppDbContext appDbContext) : GenericRepository<User>(appDbContext), IUserRepository
    {
        private readonly DbSet<User> _dbSet = appDbContext.Set<User>();
        public async Task<User> GetByUserNameOrEmailAsync(string userName, string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserName == userName || u.EmailAddress == email);
        }
    }
}
