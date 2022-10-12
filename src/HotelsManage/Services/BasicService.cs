using HotelsManage.Database;
using HotelsManage.Model;

namespace HotelsManage.Services;

public abstract class BasicService<T>
    where T : IBaseEntity<int>, new()
{
    protected BasicService()
    {
        try
        {
            Repository = new Repository<T>();
        }
        catch (Exception e)
        {
            //Program.Log.Error(e, e.Message);
            MessageBox.Show(e.Message);
            throw;
        }
        
    }

    /// <summary>
    /// Repository 实例
    /// </summary>
    protected IRepository<T> Repository { get; }
}