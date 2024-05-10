using GenTree.Domain;
using Microsoft.EntityFrameworkCore;
using WebApi.GenTree.Modules.People;
using WebApi.GenTree.Modules.Relations;

internal class Program
{
    private static async Task Main(string[] args)
    {
        (string servicePath, WebApplicationBuilder builder) = ApplicationBuilder.ConfigureBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<GenTreeContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        PeopleModule.Register(builder.Services);
        RelationsModule.Register(builder.Services);

        ApiBuilder.ConfigureApi(servicePath, builder.Services);

        var app = ApplicationBuilder.BuildApp(args, builder);

        using (var scope = app.Services.CreateScope())
        {
            using var context = scope.ServiceProvider.GetRequiredService<GenTreeContext>();
            await context.Database.MigrateAsync();
        }

        app.Run();
    }
}