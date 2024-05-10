using FluentValidation;
using GenTree.Domain;
using Microsoft.EntityFrameworkCore;
using WebApi.GenTree.Modules.People.Repositories;

namespace WebApi.GenTree.Modules.People.Validation;

public class PersonInsertRequestValidation : AbstractValidator<PersonInsertRequest>
{
    public PersonInsertRequestValidation(GenTreeContext context)
    {
        RuleFor(x => x.Given).NotEmpty().WithMessage("Имя человека должно быть заполнено");
        RuleFor(x => x.Family).NotEmpty().WithMessage("Фамилия человека должна быть заполнена");
        RuleFor(x => x.ParentId).Must(req => !req.HasValue || context.People.Any(x => x.Id == req.Value)).WithMessage("Предок не найден");
    }
}
