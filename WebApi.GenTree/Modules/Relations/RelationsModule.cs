using WebApi.GenTree.Modules.Relations.Repositories;

namespace WebApi.GenTree.Modules.Relations;

public class RelationsModule
{
    public static IServiceCollection Register(IServiceCollection services)
    {
        services.AddTransient<IRelationsInsertRepository, RelationsInsertRepository>();
        services.AddTransient<IRelationsRepository, RelationsRepository>();

        return services;
    }
}