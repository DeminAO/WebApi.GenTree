using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.GenTree.Modules.People.Repositories;

namespace WebApi.GenTree.Modules.People.Validation;

public class PeopleRequestValidation : AbstractValidator<PeopleRequest>
{
    public PeopleRequestValidation()
    {
        RuleFor(x => x.Count).GreaterThan(0).WithMessage("Количество записей на странице должно быть больше нуля");
        RuleFor(x => x.Count).LessThanOrEqualTo(1000).WithMessage("Поддерживается запрос до 1 тысячи записей на странице");
        RuleFor(x => x.Page).GreaterThanOrEqualTo(0).WithMessage("номер страницы должен содержать положительное число или ноль");
    }
}