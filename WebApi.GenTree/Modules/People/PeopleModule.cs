using WebApi.GenTree.Modules.People.Repositories;

namespace WebApi.GenTree.Modules.People;

public static class PeopleModule
{
    public static IServiceCollection Register(IServiceCollection services)
    {
        services.AddTransient<IPeopleGetRepository, PeopleGetRepository>();
        services.AddTransient<IPersonInsertRepository, PersonInsertRepository>();

        return services;
    }
}
