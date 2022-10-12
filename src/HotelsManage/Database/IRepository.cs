using System.Linq.Expressions;
using LiteDB.Async;

namespace HotelsManage.Database;

public interface IRepository
{
    ILiteDatabaseAsync Database { get; }
}

public interface IRepository<T>:IRepository
{
    /// <summary>
    /// 获取 该实体的集合
    /// </summary>
    ILiteCollectionAsync<T> Collection { get; }
    
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    ILiteQueryableAsync<T> SearchFor(Expression<Func<T, bool>> predicate);
    
    /// <summary>
    /// (异步)根据Id获取单条记录，不存在将会返回null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T?> FindAsync(object id);
    
    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    Task InsertAsync(params T[] source);
    
    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    Task InsertAsync(IReadOnlyCollection<T> source);
    
    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>删除数量</returns>
    Task<int> DeleteAsync(Expression<Func<T, bool>> filter);
    
    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(int id);
    
    /// <summary>
    /// 根据查询条件更新数据
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(Expression<Func<T, bool>> predicate, T entity);

    /// <summary>
    /// 根据实体修改数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <exception cref="ArgumentException">If the entity no exists property 'id',then will throw exception.</exception>
    Task<bool> UpdateAsync(T entity);
}