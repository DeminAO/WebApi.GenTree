using FluentValidation;
using WebApi.GenTree.Modules.Relations.Repositories;
using WebApi.GenTree.Modules.Relations.Validation;

namespace WebApi.GenTree.Modules.Relations;

public class RelationsModule
{
    public static IServiceCollection Register(IServiceCollection services)
    {
        services.AddTransient<IRelationsInsertRepository, RelationsInsertRepository>();
        services.AddTransient<IRelationsRepository, RelationsRepository>();
        services.AddScoped<IValidator<PeopleByLevelRequest>, PeopleByLevelRequestValidation>();

        return services;
    }
}