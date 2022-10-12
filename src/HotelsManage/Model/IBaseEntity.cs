namespace HotelsManage.Model;

public interface IBaseEntity{}

public interface IBaseEntity<T>:IBaseEntity
{
    public T Id { get; set; }
}

public class BaseEntity : IBaseEntity<int>
{
    public int Id { get; set; }
}