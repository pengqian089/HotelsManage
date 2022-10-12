using HotelsManage.Enum;

namespace HotelsManage.Model;

public class Room:BaseEntity
{
    /// <summary>
    /// 房号
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 单价/天
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 房间状态
    /// </summary>
    public RoomStatus Status { get; set; }

    /// <summary>
    /// 房型
    /// </summary>
    public string RoomType { get; set; }
}