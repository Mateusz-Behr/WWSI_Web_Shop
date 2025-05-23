
using Web_Shop_3.Persistence.Repositories.Interfaces;

namespace Web_Shop_3.Persistence.UOW.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository CustomerRepository { get; }
        ICategoryRepository CategoryRepository { get; }

        IGenericRepository<T> Repository<T>() where T : class;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

        Task Rollback();

        Task RollbackAsync();
    }
}
