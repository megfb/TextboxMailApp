namespace TextboxMailApp.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
