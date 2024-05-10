using FluentValidation;
using GenTree.Domain;
using Microsoft.EntityFrameworkCore;

namespace WebApi.GenTree.Modules.Relations.Validation;

public class PeopleByLevelRequestValidation : AbstractValidator<PeopleByLevelRequest>
{
	public PeopleByLevelRequestValidation(GenTreeContext context)
	{
		RuleFor(x => x.Id).Must(req => context.People.Any(x => x.Id == req)).WithMessage("Человек не найден");
	}
}