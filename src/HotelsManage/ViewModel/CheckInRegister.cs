using FluentValidation;
using Model;

namespace HotelsManage.ViewModel;

public class CheckInRegister
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

    public int Id { get; set; }

    /// <summary>
    /// 入住人数
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 实际价格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 押金
    /// </summary>
    public decimal Deposit { get; set; }

    public List<Occupant> ToOccupantList()
    {
        return new List<Occupant>
        {
            new()
            {
                Area = Area,
                From = From,
                IdCard = IdCard,
                Name = Name,
                PhoneNumber = PhoneNumber,
                Remark = Remark,
                Sex = Sex
            }
        };
    }
}

public class CheckInRegisterFluentValidator : AbstractValidator<CheckInRegister>
{
    public CheckInRegisterFluentValidator()
    {
        RuleFor(x => x.Price).NotEmpty().WithMessage("实际价格不能少于0");

        RuleFor(x => x.Deposit).NotEmpty().WithMessage("押金不得少于0");

        RuleFor(x => x.Count).NotEmpty().WithMessage("入住人数不得少于0");

        RuleFor(x => x.Name).NotEmpty().WithMessage("请输入姓名");

        RuleFor(x => x.IdCard).NotEmpty().WithMessage("请输入身份证号");

        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("请输入电话号码");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result =
            await ValidateAsync(ValidationContext<CheckInRegister>.CreateWithOptions((CheckInRegister)model,
                x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}