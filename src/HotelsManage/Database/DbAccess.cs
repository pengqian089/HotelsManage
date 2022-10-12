using HotelsManage.Model;
using LiteDB.Async;
using Serilog;

namespace HotelsManage.Database;

public class DbAccess
{
    internal DbAccess()
    {
    }

    public static readonly Lazy<LiteDatabaseAsync> _database = new(() =>
    {
        try
        {
            return new LiteDatabaseAsync("Filename=Data.db;"); //Connection=shared
        }
        catch (Exception e)
        {
            Log.Error(e,"database error");
            MessageBox.Show(e.Message);
            throw;
        }
    });

    internal ILiteDatabaseAsync Database => _database.Value;
}

internal class DbAccess<T> : DbAccess
    where T : IBaseEntity,new()
{

    internal string CollectionName => typeof(T).Name;

    internal ILiteCollectionAsync<T> Collection => Database.GetCollection<T>(CollectionName);
}