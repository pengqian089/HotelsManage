using HotelsManage.Enum;

namespace HotelsManage.Model;

/// <summary>
/// 入住记录
/// </summary>
public class HistoryRecord:BaseEntity
{

    /// <summary>
    /// 入住房间
    /// </summary>
    public int RoomId { get; set; }
    
    /// <summary>
    /// 房号
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// 房型
    /// </summary>
    public string RoomType { get; set; }
    
    /// <summary>
    /// 入住房客
    /// </summary>
    public List<int> OccupantId { get; set; }
    
    /// <summary>
    /// 房客人数
    /// </summary>
    public int OccupantCount { get; set; }
    
    /// <summary>
    /// 实际单价/天
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 押金
    /// </summary>
    public decimal Deposit { get; set; }

    /// <summary>
    /// 押金状态
    /// </summary>
    public DepositStatus DepositStatus { get; set; }

    /// <summary>
    /// 记录状态
    /// </summary>
    public RecordStatus RecordStatus { get; set; }
    
    /// <summary>
    /// 入住时间
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// 离开时间
    /// </summary>
    public DateTime? DepartureTime { get; set; }
}