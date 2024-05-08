namespace WebApi.GenTree.Modules.Relations;

public class RelationsModule
{
    public static IServiceCollection Register(IServiceCollection services)
    {
        services.AddTransient<IRelationsInsertRepository, RelationsInsertRepository>();

        return services;
    }
}
