using HotelsManage.Enum;
using Model;

namespace HotelsManage.ViewModel;

public class RoomDetail
{
    public int Id { get; set; }

    /// <summary>
    /// 房号
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 单价/天
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 房间状态
    /// </summary>
    public RoomStatus Status { get; set; }

    /// <summary>
    /// 房型
    /// </summary>
    public string? RoomType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? HistoryRecordId { get; set; }


    /// <summary>
    /// 入住房客
    /// </summary>
    public List<Occupant>? Occupants { get; set; }

    /// <summary>
    /// 实际单价/天
    /// </summary>
    public decimal? ActualPrice { get; set; }

    /// <summary>
    /// 押金
    /// </summary>
    public decimal? Deposit { get; set; }

    /// <summary>
    /// 押金状态
    /// </summary>
    public DepositStatus? DepositStatus { get; set; }

    /// <summary>
    /// 记录状态
    /// </summary>
    public RecordStatus? RecordStatus { get; set; }

    /// <summary>
    /// 入住时间
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// 离开时间
    /// </summary>
    public DateTime? DepartureTime { get; set; }

    public bool ShowOccupant { get; set; }

}