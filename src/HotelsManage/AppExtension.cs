using FluentValidation;
using HotelsManage.Enum;
using HotelsManage.Model;
using Microsoft.AspNetCore.Components;

namespace HotelsManage;

public static class AppExtension
{
    public const string AppName = "管理系统";
    
    
    
    public static Func<object, string, Task<IEnumerable<string>>> ValidateValue<T>(this T t)
        where T:AbstractValidator<T>
    {
        return async (model, propertyName) =>
        {
            var result =
                await t.ValidateAsync(ValidationContext<T>.CreateWithOptions((T)model,
                    x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}