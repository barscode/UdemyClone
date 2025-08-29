using System.Linq.Expressions;

namespace UdemyClone.Api.Infrastructure.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Query();
    Task<T?> GetByIdAsync(params object[] keyValues);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<int> SaveChangesAsync();
}
