using TextboxMailApp.Application.Contracts.Persistence;

namespace TextboxMailApp.Persistence
{
    public class UnitOfWork(AppDbContext appDbContext) : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

        public void Dispose()
        {
            _appDbContext.Dispose();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
