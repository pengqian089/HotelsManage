using HotelsManage.Enum;
using Microsoft.AspNetCore.Components;

namespace HotelsManage;

public static class AppExtension
{
    public const string AppName = "管理系统";
    
    public static MarkupString ToDisplayName(this RoomStatus status)
    {
        return status switch
        {
            RoomStatus.Empty => (MarkupString)"<span style='background:green;color:#fff;padding:1em'>空房</span>",
            RoomStatus.CheckIn => (MarkupString)"<span style='background:red;color:#000;padding:1em'>入住</span>",
            RoomStatus.Reservation => (MarkupString)"<span style='background:green;color:#fff;padding:1em'>预定</span>",
            _ => (MarkupString)""
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