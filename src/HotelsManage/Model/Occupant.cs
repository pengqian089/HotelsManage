using HotelsManage.Model;

namespace Model;

/// <summary>
/// 房客
/// </summary>
public class Occupant:BaseEntity
{

    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// 性别
    /// </summary>
    public string? Sex { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    public string? IdCard { get; set; }

    /// <summary>
    /// 地区
    /// </summary>
    public string? Area { get; set; }

    /// <summary>
    /// 来自哪里
    /// </summary>
    public string? From { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}