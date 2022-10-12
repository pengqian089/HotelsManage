using HotelsManage.Enum;

namespace HotelsManage;

public static class AppExtension
{
    public const string AppName = "管理系统";
    
    public static string ToDisplayName(this RoomStatus status)
    {
        return status switch
        {
            RoomStatus.Empty => "空房",
            RoomStatus.CheckIn => "入住",
            RoomStatus.Reservation => "预定",
            _ => ""
        };
    }

    public static string ToDisplayName(this DepositStatus depositStatus)
    {
        return depositStatus switch
        {
            DepositStatus.Pay => "已支付",
            DepositStatus.Returned => "已退还",
            DepositStatus.Unreturned => "未退还",
            DepositStatus.NotCharged => "未收取",
            _ => ""
        };
    }
    
    public static string ToDisplayName(this RecordStatus recordStatus)
    {
        return recordStatus switch
        {
            RecordStatus.Complete => "已完成",
            RecordStatus.CheckIn => "已入住",
            _ => ""
        };
    }
}