using FluentValidation;
using WebApi.GenTree.Modules.People.Repositories;
using WebApi.GenTree.Modules.People.Validation;

namespace WebApi.GenTree.Modules.People;

public static class PeopleModule
{
    public static IServiceCollection Register(IServiceCollection services)
    {
        services.AddTransient<IPeopleGetRepository, PeopleGetRepository>();
        services.AddTransient<IPersonInsertRepository, PersonInsertRepository>();

        services.AddScoped<IValidator<PeopleRequest>, PeopleRequestValidation>();
        services.AddScoped<IValidator<PersonInsertRequest>, PersonInsertRequestValidation>();

        return services;
    }
}
