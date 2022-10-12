namespace HotelsManage.Enum;

/// <summary>
/// 押金
/// </summary>
public enum DepositStatus
{
    /// <summary>
    /// 未收取
    /// </summary>
    NotCharged,
    
    /// <summary>
    /// 未退还
    /// </summary>
    Unreturned,
    
    /// <summary>
    /// 已支付
    /// </summary>
    Pay,
    
    /// <summary>
    /// 已退还
    /// </summary>
    Returned
}