using System.Linq.Expressions;
using HotelsManage.Model;
using LiteDB.Async;

namespace HotelsManage.Database;

public class Repository<T> : IRepository<T>
    where T : IBaseEntity<int>, new()
{
    private readonly DbAccess<T> _access;
    
    public ILiteDatabaseAsync Database => _access.Database;
    
    public ILiteCollectionAsync<T> Collection => _access.Collection;

    public Repository()
    {
        _access = new DbAccess<T>();
    }

    public ILiteQueryableAsync<T> SearchFor(Expression<Func<T, bool>> predicate)
    {
        return Collection.Query().Where(predicate);
    }

    public async Task<T?> FindAsync(object id)
    {
        var property = typeof(T).GetProperty("Id");
        if (property == null) return default(T);
        var parameter = Expression.Parameter(typeof(T), "__q");
        var memberExpr = Expression.Property(parameter, property);
        var expression = Expression.Equal(memberExpr, Expression.Constant(id));
        var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);
        
        return await Collection.Query().Where(lambda).SingleOrDefaultAsync();
    }

    public async Task InsertAsync(params T[] source)
    {
        if(source.Any())
            await Collection.InsertAsync(source);
    }

    public async Task InsertAsync(IReadOnlyCollection<T> source)
    {
        if(source.Any())
            await Collection.InsertAsync(source);
    }

    public async Task<int> DeleteAsync(Expression<Func<T, bool>> filter)
    {
        return await Collection.DeleteManyAsync(filter);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await Collection.DeleteAsync(id);
    }

    public async Task<int> UpdateAsync(Expression<Func<T, bool>> predicate, T entity)
    {
        return await Collection.UpdateManyAsync(x => entity, predicate);
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        return await Collection.UpdateAsync(entity);
    }
}