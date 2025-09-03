using TextboxMailApp.Application.Features.EmailMessages;

namespace TextboxMailApp.Application.Contracts.Persistence
{
    public interface IGenericRepository<T>
    {
        Task<T> CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllByPageAsync(int pageNumber, int pageSize);
        Task SaveRangeAsync(IEnumerable<T> entity);

    }
}
