using Microsoft.EntityFrameworkCore;
using UdemyClone.Api.Data;

namespace UdemyClone.Api.Infrastructure.Repositories;

public class EfGenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    private readonly DbSet<T> _set;

    public EfGenericRepository(ApplicationDbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public IQueryable<T> Query() => _set.AsQueryable();

    public Task<T?> GetByIdAsync(params object[] keyValues) => _set.FindAsync(keyValues).AsTask();

    public Task AddAsync(T entity) { _set.Add(entity); return Task.CompletedTask; }

    public void Update(T entity) => _set.Update(entity);

    public void Remove(T entity) => _set.Remove(entity);

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}
