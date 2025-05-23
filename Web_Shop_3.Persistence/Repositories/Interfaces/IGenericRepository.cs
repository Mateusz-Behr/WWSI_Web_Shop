

namespace Web_Shop_3.Persistence.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IGenericRepository<T> WithTracking();             // <-- Defaultowo
        IGenericRepository<T> WithoutTracking();          // <-- Jak chcemy dane pobrać tylko do wyświetlenie - żeby było wydajniej.
                                                // Metody związane z samą charakterystyką EF - jeśli pobierzemy jakiś obiekt z BD
                                                // to on w swojej wewn. strukturze pamięciowej opakowuje go w odpowiednie elementy,
                                                // które służą zarządzaniu takim obiektem.

        IQueryable<T> Entities { get; }

        Task<T> AddAsync(T entity);                             // C
        Task<T?> GetByIdAsync(params object?[]? id);            // R
        Task<T> UpdateAsync(T entity, params object?[]? id);    // U
        Task DeleteAsync(T entity);                             // D
        Task<bool> Exists(params object?[]? id);        // Czy istnieje w BD?
    }
}
