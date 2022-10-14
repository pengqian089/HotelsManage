using FluentValidation;
using HotelsManage.Enum;
using HotelsManage.ViewModel;

namespace HotelsManage.Model;

public class Room:BaseEntity
{
    /// <summary>
    /// 房号
    /// </summary>
    public string? Name { get; set; }

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
    public string? RoomType { get; set; }
}

public class RoomFluentValidator : Validator<Room>
{
    public RoomFluentValidator()
    {
        RuleFor(x => x.Price).NotEmpty().WithMessage("实际价格不能少于0");

        RuleFor(x => x.Name).NotEmpty().WithMessage("请输入房号");

        RuleFor(x => x.RoomType).NotEmpty().WithMessage("请输入房型");
    }
}